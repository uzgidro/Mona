using Microsoft.AspNetCore.SignalR;
using Mona.Enum;
using Mona.Model.Dto;

namespace Mona.Hub;

public abstract class MainHub : Hub<IHubInterfaces>
{
    protected string GetSender()
    {
        return Context.User!.Claims.First(claim => claim.Type.Equals(Claims.Id)).Value;
    }

    protected IHubInterfaces SetRoute(MessageDto message)
    {
        if (message.ChatId.StartsWith("g-") || message.ChatId.StartsWith("c-"))
        {
            return Clients.Group(message.ChatId);
        }
        else
        {
            return Clients.Users(message.ReceiverId,
                message.SenderId);
        }
    }
}