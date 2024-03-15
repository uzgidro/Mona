using Microsoft.AspNetCore.SignalR;
using Mona.Enum;

namespace Mona.Config;

public class UserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User.Claims.First(item => item.Type.Equals(Claims.Username)).Value;
    }
}