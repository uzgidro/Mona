using Microsoft.AspNetCore.Authorization;
using Mona.Model.Dto;
using Mona.Service.Interface;

namespace Mona.Hub;

[Authorize]
public class GroupHub(IMessageService service, IUserService userService, IGroupService groupService) : MainHub
{
    public override async Task OnConnectedAsync()
    {
        var userGroupList = await groupService.GetUserGroupList(GetSender());
        foreach (var groupModel in userGroupList)
        {
            await Groups.AddToGroupAsync(GetSender(), groupModel.Id);
        }

        await base.OnConnectedAsync();
    }

    public async Task CreateGroup(GroupRequest request)
    {
        try
        {
            var userId = GetSender();
            if (string.IsNullOrEmpty(userId)) await Clients.Caller.ReceiveException(new UnauthorizedAccessException());
            request.CreatorId = userId;
            var group = await groupService.CreateGroup(request);
            foreach (var user in group.Users)
            {
                await Groups.AddToGroupAsync(user.Id, group.Id);
                await Clients.User(user.Id).AppendMember(group);
            }
        }
        catch (Exception e)
        {
            await Clients.Caller.ReceiveException(e);
        }
    }

    public async Task EditGroup(string groupId, GroupRequest request)
    {
        try
        {
            var group = await groupService.EditGroup(groupId, request);
            await Clients.Group(group.Id).EditGroup(group);
        }
        catch (Exception e)
        {
            await Clients.Caller.ReceiveException(e);
        }
    }

    public async Task AppendMembers(string groupId, IEnumerable<string> membersId)
    {
        try
        {
            var members = await groupService.AddMembers(groupId, membersId);
            foreach (var userGroup in members)
            {
                await Groups.AddToGroupAsync(userGroup.UserId, userGroup.GroupId);
                await Clients.User(userGroup.UserId).AppendMember(userGroup.GroupModel);
            }
        }
        catch (Exception e)
        {
            await Clients.Caller.ReceiveException(e);
        }
    }

    public async Task RemoveMembers(string groupId, IEnumerable<string> membersId)
    {
        try
        {
            var members = await groupService.RemoveMembers(groupId, membersId);
            foreach (var userGroup in members)
            {
                await Groups.RemoveFromGroupAsync(userGroup.UserId, userGroup.GroupId);
                await Clients.User(userGroup.UserId).RemoveMember(userGroup.GroupModel);
            }
        }
        catch (Exception e)
        {
            await Clients.Caller.ReceiveException(e);
        }
    }

    public async Task LeaveGroup(string groupId)
    {
        try
        {
            var id = GetSender();
            var members = await groupService.LeaveGroup(groupId, id);
            await Groups.RemoveFromGroupAsync(id, groupId);
            await Clients.Caller.RemoveMember(members.GroupModel);
        }
        catch (Exception e)
        {
            await Clients.Caller.ReceiveException(e);
        }
    }

    public async Task DeleteGroup(string groupId)
    {
        try
        {
            var userGroups = await groupService.DeleteGroup(groupId);
            foreach (var userGroup in userGroups)
            {
                await Groups.RemoveFromGroupAsync(userGroup.UserId, userGroup.GroupId);
                await Clients.User(userGroup.UserId).RemoveMember(userGroup.GroupModel);
            }
        }
        catch (Exception e)
        {
            await Clients.Caller.ReceiveException(e);
        }
    }
}