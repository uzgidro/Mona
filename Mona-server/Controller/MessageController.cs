using Microsoft.AspNetCore.Mvc;
using Mona.Service.Interface;
using Mona.Utilities;

namespace Mona.Controller;

// [Authorize]
[ApiController]
[Route("[controller]")]
public class MessageController(IFileService fileService) : ControllerBase
{
    [HttpPost("send")]
    [RequestSizeLimit(2147483648)] // 2 GB
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [MultipartFormData]
    [DisableFormValueModelBinding]
    public async Task<IResult> Post()
    {
        var fileUploadSummary = await fileService.UploadFileAsync(HttpContext.Request.Body, Request.ContentType);
        return Results.Created(nameof(Post), fileUploadSummary);
    }
}