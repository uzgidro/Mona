using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mona.Context;

namespace Mona.Controller;

[Authorize]
[ApiController]
[Route("[controller]")]
public class FilesController(ApplicationContext context) : ControllerBase
{
    [HttpGet("download/{id}")]
    public async Task<IActionResult> DownloadFile(string id)
    {
        var fileInfo = await context.Files
            .AsNoTracking()
            .Where(item => item.Id.Equals(id) && !item.IsDeleted)
            .FirstOrDefaultAsync();
        if (fileInfo == null || !System.IO.File.Exists(fileInfo.Path)) return NotFound();
        await using var fileStream = new FileStream(fileInfo.Path, FileMode.Open, FileAccess.Read);

        return File(fileStream, "application/octet-stream", fileInfo.Name);
    }
}