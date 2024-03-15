using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Mona.Model;
using Mona.Service.Interface;

namespace Mona.Hub;

[Authorize]
public class SimpleHub(IMessageService service) : Hub<IHubInterfaces>
{
    public async Task Send(string message)
    {
        // service.CreateMessage(message);
        await Clients.User("Abbos").ReceiveMessage(message);
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
}