using Microsoft.AspNetCore.Authorization;
using Mona.Model;
using Mona.Model.Dto;
using Mona.Service.Interface;
using Mona.Utilities;

namespace Mona.Hub;

[Authorize]
public class ChatHub(IMessageService service, IUserService userService)
    : MainHub
{
    public async Task SendMessage(MessageRequest message)
    {
        try
        {
            var messageItem = await service.CreateMessage(message.ToMessageModel(GetSender()));
            var activeMessage = await service.ActiveMessage(messageItem);
            await SetRoute(messageItem).ReceiveMessage(activeMessage);
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

    public async Task DeleteMessageForMyself(MessageModel message)
    {
        try
        {
            var deleted = await service.DeleteMessageForMyself(GetSender(), message);
            await Clients.Caller.DeleteMessage(deleted);
        }
        catch (Exception e)
        {
            await Clients.Caller.ReceiveException(e);
        }
    }

    public async Task DeleteMessageForEveryone(MessageModel message)
    {
        var deleted = await service.DeleteMessageForEveryone(GetSender(), message);
        await SetRoute(deleted).DeleteMessage(message);
    }

    public async Task<IEnumerable<UserModel>> GetUsers()
    {
        var userManagerUsers = await userService.GetUsersExceptCaller(GetSender());
        return userManagerUsers;
    }

    public async Task<IEnumerable<MessageModel>> GetHistory()
    {
        var messages = await service.GetMessages(GetSender());
        return messages;
    }
}