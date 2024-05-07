using Mona.Service.Interface;

namespace Mona.Hub;

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
}