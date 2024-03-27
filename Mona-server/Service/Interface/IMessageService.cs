using Mona.Model;
using Mona.Model.Dto;

namespace Mona.Service.Interface;

public interface IMessageService
{
    Task<MessageItem> CreateMessage(MessageRequest message);
    Task<MessageItem?> EditMessage(string? caller, MessageItem message);
    Task<MessageItem?> DeleteMessageForMyself(string? caller, MessageItem message);
    Task<MessageItem?> DeleteMessageForEveryone(string? caller, MessageItem message);
    Task<IEnumerable<MessageItem>> GetMessages(string? caller);
}