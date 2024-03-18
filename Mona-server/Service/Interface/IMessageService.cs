using Mona.Model;
using Mona.Model.Dto;

namespace Mona.Service.Interface;

public interface IMessageService
{
    Task<MessageItem> CreateMessage(MessageRequest message);
    Task<IEnumerable<MessageItem>> GetMessages();
}