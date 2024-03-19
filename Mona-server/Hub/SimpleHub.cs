using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Mona.Enum;
using Mona.Model;
using Mona.Model.Dto;
using Mona.Service.Interface;

namespace Mona.Hub;

[Authorize]
public class SimpleHub(IMessageService service, IUserService userService) : Hub<IHubInterfaces>
{
    public async Task SendDirectMessage(MessageRequest message)
    {
        message.SenderId = GetSender();
        var messageItem = await service.CreateMessage(message);
        await Clients.Users(message.ReceiverId, message.SenderId).ReceiveMessage(messageItem);
    }

    public async Task<IEnumerable<ApplicationUser>> GetUsers()
    {
        var sender = GetSender();
        var userManagerUsers = await userService.GetUsersExceptCaller(sender);
        return userManagerUsers;
    }

    public async Task<IEnumerable<MessageItem>> GetHistory()
    {
        var messages = await service.GetMessages(GetSender());
        return messages;
    }

    public async Task JoinGroup(string group)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, group);
    }

    private string? GetSender()
    {
        return Context.User?.Claims.First(claim => claim.Type.Equals(Claims.Id)).Value;
    }
}