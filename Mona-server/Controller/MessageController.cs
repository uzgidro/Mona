using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
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
    IHubContext<ChatHub, IHubInterfaces> hubContext) : MainController
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
            var userId = GetUserId();
            var boundary = GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType));
            var multipartReader = new MultipartReader(boundary, HttpContext.Request.Body);

            var messageModel = await messageService.CreateMessage(multipartReader, userId);

            // Cannot append files to forward
            if (messageModel.Forward == null)
                await fileService.UploadFileAsync(multipartReader, messageModel);

            var activeMessage = await messageService.ActiveMessage(messageModel.Id);
            await hubContext.Clients.Groups(activeMessage.ChatId)
                .ReceiveMessage(activeMessage);

            if (activeMessage.ChatId.StartsWith("g-") || activeMessage.ChatId.StartsWith("c-"))
            {
                await hubContext.Clients.Group(activeMessage.ChatId)
                    .ReceiveMessage(activeMessage);
            }
            else
            {
                await hubContext.Clients
                    .Users(
                        activeMessage.ReceiverId,
                        activeMessage.SenderId)
                    .ReceiveMessage(activeMessage);
            }

            return Created(nameof(Post), messageModel);
        }
        // If message named part not provided
        catch (NullReferenceException e)
        {
            await hubContext.Clients.User(GetUserId()).ReceiveException(e);
            return BadRequest(e.Message);
        }
        catch (ArgumentNullException e)
        {
            await hubContext.Clients.User(GetUserId()).ReceiveException(e);
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            await hubContext.Clients.User(GetUserId()).ReceiveException(e);
            return BadRequest(e.Message);
        }
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetMessages()
    {
        return Ok(await messageService.GetChats(GetUserId()));
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