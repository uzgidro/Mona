using Mona.Model;
using Mona.Model.Dto;

namespace Mona.Service.Interface;

public interface IMessageService
{
    Task<MessageModel> CreateMessage(MessageRequest message);
    Task<MessageModel?> EditMessage(string? caller, MessageModel message);
    Task<MessageModel?> DeleteMessageForMyself(string? caller, MessageModel message);
    Task<MessageModel?> DeleteMessageForEveryone(string? caller, MessageModel message);
    Task<IEnumerable<MessageModel>> GetMessages(string? caller);
}