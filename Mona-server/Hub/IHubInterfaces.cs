using Mona.Model;

namespace Mona.Hub;

public interface IHubInterfaces
{
    Task ReceiveMessage(MessageModel message);
    Task ModifyMessage(MessageModel message);
    Task DeleteMessage(string messageId);
    Task PinMessage(MessageModel message);
    Task ReceiveException(Exception e);
    Task EditGroup(GroupModel group);
    Task AppendMember(GroupModel group);
    Task RemoveMember(GroupModel group);
}