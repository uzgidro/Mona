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
    public async Task<IResult> Post()
    {
        var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type.Equals(Claims.Id))?.Value;
        if (string.IsNullOrEmpty(userId)) return Results.BadRequest();
        var boundary = GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType));
        var multipartReader = new MultipartReader(boundary, HttpContext.Request.Body);

        var messageModel = await messageService.CreateMessage(multipartReader, userId);
        if (messageModel == null) return Results.BadRequest();

        var fileUploadSummary =
            await fileService.UploadFileAsync(multipartReader, messageModel.Id);

        var activeMessage = await messageService.ActiveMessage(messageModel);
        hubContext.Clients.Users(activeMessage.ReceiverId, activeMessage.SenderId).ReceiveMessage(activeMessage);
        return Results.Created(nameof(Post), fileUploadSummary);
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