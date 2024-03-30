using Mona.Model;

namespace Mona.Hub;

public interface IHubInterfaces
{
    Task ReceiveMessage(MessageModel message);
    Task ModifyMessage(MessageModel message);
    Task DeleteMessage(MessageModel message);
}