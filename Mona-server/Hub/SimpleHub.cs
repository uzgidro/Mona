using Microsoft.AspNetCore.SignalR;
using Mona.Model;
using Mona.Service;

namespace Mona.Hub;

public class SimpleHub(IMessageService service) : Hub<IHubInterfaces>
{
    public async Task Send(MessageItem message)
    { 
        service.CreateMessage(message);
        await Clients.Group(message.Group).ReceiveMessage(message);
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