using Microsoft.AspNetCore.Authorization;
using Mona.Model;
using Mona.Model.Dto;
using Mona.Service.Interface;

namespace Mona.Hub;

[Authorize]
public class ChatHub(IMessageService service, IUserService userService)
    : MainHub
{
    public async Task SendMessage(MessageRequest message)
    {
        try
        {
            var messageItem = await service.CreateMessage(message, GetSender());
            var activeMessage = await service.ActiveMessage(messageItem.Id);
            await SetRoute(activeMessage).ReceiveMessage(activeMessage);
        }
        catch (Exception e)
        {
            await Clients.Caller.ReceiveException(e);
        }
    }

    public async Task EditMessage(MessageModel message)
    {
        try
        {
            var edited = await service.EditMessage(GetSender(), message);
            await SetRoute(edited).ModifyMessage(edited);
        }
        catch (Exception e)
        {
            await Clients.Caller.ReceiveException(e);
        }
    }

    public async Task PinMessage(string messageId)
    {
        try
        {
            var pinned = await service.PinMessage(messageId);
            await SetRoute(pinned).PinMessage(pinned);
        }
        catch (Exception e)
        {
            await Clients.Caller.ReceiveException(e);
        }
    }

    public async Task DeleteMessageForMyself(string messageId)
    {
        try
        {
            var deleted = await service.DeleteMessageForMyself(GetSender(), messageId);
            await Clients.Caller.DeleteMessage(deleted.Id);
        }
        catch (Exception e)
        {
            await Clients.Caller.ReceiveException(e);
        }
    }

    public async Task DeleteMessageForEveryone(string messageId)
    {
        var deleted = await service.DeleteMessageForEveryone(GetSender(), messageId);
        await SetRoute(deleted).DeleteMessage(deleted.Id);
    }

    public async Task<IEnumerable<UserModel>> GetUsers()
    {
        var userManagerUsers = await userService.GetUsersExceptCaller(GetSender());
        return userManagerUsers;
    }

    // public async Task<IEnumerable<MessageModel>> GetHistory()
    // {
    //     var messages = await service.GetMessages(GetSender());
    //     return messages;
    // }

    public async Task<IEnumerable<ChatResponse>> GetChats()
    {
        return await service.GetChats(GetSender());
    }

    public async Task<IEnumerable<MessageDto>> GetChatMessages(string chatId)
    {
        return await service.GetChatMessages(GetSender(), chatId);
    }
}