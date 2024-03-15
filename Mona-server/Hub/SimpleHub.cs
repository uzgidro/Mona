using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Mona.Enum;
using Mona.Model;
using Mona.Service.Interface;

namespace Mona.Hub;

[Authorize]
public class SimpleHub(IMessageService service, IUserService userService) : Hub<IHubInterfaces>
{
    public async Task SendDirectMessage(string receiverUsername, string message)
    {
        // service.CreateMessage(message);
        var sender = GetSender();
        await Clients.User(receiverUsername).ReceiveMessage(sender, message);
    }

    public async Task<IEnumerable<UserDto>> GetUsers()
    {
        var sender = GetSender();
        var userManagerUsers = await userService.GetUsersExceptCaller(sender);
        return userManagerUsers;
    }

    public async Task<IEnumerable<MessageItem>> GetHistory(string group)
    {
        var messages = await service.GetMessagesByGroup(group);
        return messages;
    }

    public async Task JoinGroup(string group)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, group);
    }

    private string? GetSender()
    {
        return Context.User?.Claims.First(claim => claim.Type.Equals(Claims.Username)).Value;
    }
}