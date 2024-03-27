using Mona.Model;
using Mona.Model.Dto;

namespace Mona.Service.Interface;

public interface IMessageService
{
    Task<MessageItem> CreateMessage(MessageRequest message);
    Task<MessageItem?> EditMessage(MessageItem message);
    Task<MessageItem?> DeleteMessageForMyself(MessageItem message);
    Task<MessageItem?> DeleteMessageForEveryone(MessageItem message);
    Task<IEnumerable<MessageItem>> GetMessages(string caller);
}