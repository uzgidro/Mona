using Microsoft.AspNetCore.SignalR;
using Mona.Enum;
using Mona.Model;

namespace Mona.Hub;

public abstract class MainHub : Hub<IHubInterfaces>
{
    protected string GetSender()
    {
        return Context.User!.Claims.First(claim => claim.Type.Equals(Claims.Id)).Value;
    }

    protected IHubInterfaces SetRoute(MessageModel message)
    {
        if (!string.IsNullOrEmpty(message.DirectReceiverId))
        {
            return Clients.Users(message.DirectReceiverId, message.SenderId);
        }

        if (!string.IsNullOrEmpty(message.GroupReceiverId))
        {
            return Clients.Groups(message.GroupReceiverId);
        }

        throw new ArgumentException("Invalid message: No direct or group receiver specified.");
    }
}