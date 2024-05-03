using Microsoft.AspNetCore.WebUtilities;
using Mona.Model;

namespace Mona.Service.Interface;

public interface IMessageService
{
    // Task<MessageModel> CreateMessage(MessageRequest message);
    Task<MessageModel> CreateMessage(MultipartReader multipartReader, string senderId);
    Task<MessageModel> ActiveMessage(MessageModel messageModel);
    Task<MessageModel> EditMessage(string? caller, MessageModel message);
    Task<MessageModel> DeleteMessageForMyself(string? caller, MessageModel message);
    Task<MessageModel> DeleteMessageForEveryone(string? caller, MessageModel message);
    Task<IEnumerable<MessageModel>> GetMessages(string? caller);
}