using Microsoft.AspNetCore.WebUtilities;
using Mona.Model;

namespace Mona.Service.Interface;

public interface IMessageService
{
    Task<MessageModel> CreateMessage(MessageModel message);
    Task<MessageModel> CreateMessage(MultipartReader multipartReader, string senderId);
    Task<MessageModel> ActiveMessage(string messageId);
    Task<MessageModel> EditMessage(string caller, MessageModel message);
    Task<MessageModel> DeleteMessageForMyself(string caller, string messageId);
    Task<MessageModel> DeleteMessageForEveryone(string caller, string messageId);
    Task<IEnumerable<MessageModel>> GetMessages(string caller);
    Task<List<ChatModel>> GetUserChatsAsync(string caller);
    Task<MessageModel> PinMessage(string messageId);
}