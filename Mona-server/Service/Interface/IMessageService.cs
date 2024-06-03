using Microsoft.AspNetCore.WebUtilities;
using Mona.Model;
using Mona.Model.Dto;

namespace Mona.Service.Interface;

public interface IMessageService
{
    Task<MessageModel> CreateMessage(MessageRequest message, string senderId);
    Task<MessageModel> CreateMessage(MultipartReader multipartReader, string senderId);
    Task<MessageModel> ActiveMessage(string messageId);
    Task<MessageModel> EditMessage(string caller, MessageModel message);
    Task<MessageModel> DeleteMessageForMyself(string caller, string messageId);

    Task<MessageModel> DeleteMessageForEveryone(string caller, string messageId);

    // Task<IEnumerable<MessageModel>> GetMessages(string caller);
    Task<List<ChatResponse>> GetChats(string caller);
    Task<List<MessageDto>> GetChatMessages(string caller, string chatId);
    Task<MessageModel> PinMessage(string messageId);
}