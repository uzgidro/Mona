using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Mona.Hub;
using Mona.Model.Dto;
using Mona.Service.Interface;

namespace Mona.Controller;

[Authorize]
[ApiController]
[Route("[controller]")]
public class GroupController(
    IGroupService service,
    IHubContext<ChatHub, IHubInterfaces> hubContext) : MainController
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateGroup(GroupRequest request)
    {
        try
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            request.CreatorId = userId;
            var group = await service.CreateGroup(request);
            foreach (var user in group.Users)
            {
                await hubContext.Groups.AddToGroupAsync(user.Id, group.Id);
            }

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
            foreach (var userGroup in members)
            {
                await hubContext.Groups.AddToGroupAsync(userGroup.UserId, userGroup.GroupId);
            }

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
            foreach (var userGroup in members)
            {
                await hubContext.Groups.RemoveFromGroupAsync(userGroup.UserId, userGroup.GroupId);
            }

            return Ok(members);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpPost("{groupId}/leave")]
    public async Task<IActionResult> LeaveGroup(string groupId)
    {
        try
        {
            var id = GetUserId();
            var members = await service.LeaveGroup(groupId, id);
            await hubContext.Groups.RemoveFromGroupAsync(id, groupId);
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
            var userGroups = await service.DeleteGroup(groupId);
            foreach (var userGroup in userGroups)
            {
                await hubContext.Groups.RemoveFromGroupAsync(userGroup.UserId, userGroup.GroupId);
            }

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
            await service.GetUserGroupList(GetUserId());
        return Ok(userGroupList);
    }

    [HttpGet("{groupId}")]
    public async Task<IActionResult> GetGroupInfo(string groupId)
    {
        return Ok(await service.GetGroupInfo(groupId));
    }
}