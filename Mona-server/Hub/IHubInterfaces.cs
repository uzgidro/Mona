using Mona.Model;

namespace Mona.Hub;

public interface IHubInterfaces
{
    Task ReceiveMessage(MessageItem message);
    Task ModifyMessage(MessageItem message);
    Task DeleteMessage(MessageItem message);
}