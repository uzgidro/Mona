using Mona.Model.Dto;

namespace Mona.Hub;

public interface IHubInterfaces
{
    Task ReceiveMessage(MessageDto message);
    Task ModifyMessage(MessageDto message);
    Task DeleteMessage(string messageId);
    Task PinMessage(MessageDto message);
    Task ReceiveException(Exception e);
    Task UpdateChat(ChatDto chat);
    Task RemoveChat(string chatId);
}