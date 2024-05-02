using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mona.Enum;
using Mona.Model.Dto;
using Mona.Service.Interface;

namespace Mona.Controller;

[Authorize]
[ApiController]
[Route("[controller]")]
public class GroupController(IGroupService service) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateGroup(GroupRequest request)
    {
        var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type.Equals(Claims.Id))?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();
        request.CreatorId = userId;
        var group = await service.CreateGroup(request);
        return Ok(group);
    }

    [HttpPut("{groupId}")]
    public async Task<IActionResult> EditGroup(string groupId, GroupRequest request)
    {
        try
        {
            var group = await service.EditGroup(groupId, request);
            return Ok(group);
        }
        catch (System.Exception)
        {
            return NotFound();
        }
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserWithGroup()
    {
        // var aaa = await context.Users.Include(model => model.Groups).ToListAsync();
        // return Ok(aaa);
        return Ok();
    }
}