using Microsoft.AspNetCore.WebUtilities;
using Mona.Model.Dto;

namespace Mona.Service.Interface;

public interface IMessageService
{
    Task<MessageDto> CreateMessage(MessageRequest message, string senderId);
    Task<MessageDto> CreateMessage(MultipartReader multipartReader, string senderId);
    Task<MessageDto> ActiveMessage(string messageId);
    Task<MessageDto> EditMessage(string caller, MessageRequest message);
    Task<MessageDto> DeleteMessageForMyself(string caller, string messageId);

    Task<MessageDto> DeleteMessageForEveryone(string caller, string messageId);

    // Task<IEnumerable<MessageModel>> GetMessages(string caller);
    Task<List<ChatDto>> GetChats(string caller);
    Task<ChatDto> GetChatWithLastMessage(string chatId, string caller);
    Task<List<MessageDto>> GetMessagesByChatId(string caller, string chatId);
    Task<List<MessageDto>> GetMessagesByUserId(string caller, string userId);
    Task<MessageDto> PinMessage(string messageId);
}