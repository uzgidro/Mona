using Mona.Model;

namespace Mona.Service.Interface;

public interface IMessageService
{ 
    void CreateMessage(MessageItem message);
    Task<IEnumerable<MessageItem>> GetMessages();
    Task<IEnumerable<MessageItem>> GetMessagesByGroup(string group);
}