using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Mona.Enum;
using Mona.Hub;
using Mona.Service.Interface;
using Mona.Utilities;

namespace Mona.Controller;

[Authorize]
[ApiController]
[Route("[controller]")]
public class MessageController(
    IFileService fileService,
    IMessageService messageService,
    IHubContext<SimpleHub, IHubInterfaces> hubContext) : ControllerBase
{
    [HttpPost("send")]
    [RequestSizeLimit(2147483648)] // 2 GB
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [MultipartFormData]
    [DisableFormValueModelBinding]
    public async Task<IActionResult> Post()
    {
        try
        {
            var userId = HttpContext.User.Claims.First(claim => claim.Type.Equals(Claims.Id)).Value;
            var boundary = GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType));
            var multipartReader = new MultipartReader(boundary, HttpContext.Request.Body);

            var messageModel = await messageService.CreateMessage(multipartReader, userId);

            var fileUploadSummary =
                await fileService.UploadFileAsync(multipartReader, messageModel.Id);

            var activeMessage = await messageService.ActiveMessage(messageModel);
            if (!string.IsNullOrEmpty(activeMessage.DirectReceiverId))
            {
                await hubContext.Clients.Users(activeMessage.DirectReceiverId, activeMessage.SenderId)
                    .ReceiveMessage(activeMessage);
            }
            else if (!string.IsNullOrEmpty(activeMessage.GroupReceiverId))
            {
                await hubContext.Clients.Groups(activeMessage.GroupReceiverId)
                    .ReceiveMessage(activeMessage);
            }
            else
            {
                return BadRequest("Message did not sent");
            }

            return CreatedAtRoute(nameof(Post), fileUploadSummary);
        }
        catch (NullReferenceException e)
        {
            return BadRequest(e.Message);
        }
        catch (ArgumentNullException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    private static string GetBoundary(MediaTypeHeaderValue contentType)
    {
        var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

        if (string.IsNullOrWhiteSpace(boundary))
        {
            throw new InvalidDataException("Missing content-type boundary.");
        }

        return boundary;
    }
}