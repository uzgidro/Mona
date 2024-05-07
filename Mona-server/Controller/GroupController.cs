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
        try
        {
            var userId = HttpContext.User.Claims.First(claim => claim.Type.Equals(Claims.Id)).Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            request.CreatorId = userId;
            var group = await service.CreateGroup(request);
            return Ok(group);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{groupId}")]
    public async Task<IActionResult> EditGroup(string groupId, GroupRequest request)
    {
        try
        {
            var group = await service.EditGroup(groupId, request);
            return Ok(group);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpPost("{groupId}/append")]
    public async Task<IActionResult> AppendMembers(string groupId, IEnumerable<string> membersId)
    {
        try
        {
            var members = await service.AddMembers(groupId, membersId);
            return Ok(members);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpPost("{groupId}/remove")]
    public async Task<IActionResult> RemoveMembers(string groupId, IEnumerable<string> membersId)
    {
        try
        {
            var members = await service.RemoveMembers(groupId, membersId);
            return Ok(members);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpDelete("{groupId}")]
    public async Task<IActionResult> DeleteGroup(string groupId)
    {
        try
        {
            await service.DeleteGroup(groupId);
            return NoContent();
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetUserWithGroup()
    {
        var userGroupList =
            await service.GetUserGroupList(HttpContext.User.Claims.First(claim => claim.Type.Equals(Claims.Id)).Value);
        return Ok(userGroupList);
    }

    [HttpGet("{groupId}")]
    public async Task<IActionResult> GetGroupInfo(string groupId)
    {
        return Ok(await service.GetGroupInfo(groupId));
    }
}