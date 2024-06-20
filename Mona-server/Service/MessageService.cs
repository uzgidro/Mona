using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Mona.Context;
using Mona.Model;
using Mona.Model.Dto;
using Mona.Service.Interface;
using Mona.Utilities;

namespace Mona.Service;

public class MessageService(ApplicationContext context) : IMessageService
{
    private static readonly JsonSerializerOptions? Options = new() { PropertyNameCaseInsensitive = true };

    public async Task<MessageDto> CreateMessage(MessageRequest message, string senderId)
    {
        return await SaveMessage(message, senderId);
    }

    public async Task<MessageDto> CreateMessage(MultipartReader multipartReader, string senderId)
    {
        var section = await multipartReader.ReadNextSectionAsync();

        while (section != null)
        {
            if (section.Headers != null && section.Headers.ContainsValue("form-data; name=\"message\""))
            {
                using var reader = new StreamReader(section.Body, Encoding.UTF8);
                var messageJson = await reader.ReadToEndAsync();
                var message = JsonSerializer.Deserialize<MessageRequest>(messageJson, Options);

                return await SaveMessage(message, senderId, false);
            }

            section = await multipartReader.ReadNextSectionAsync();
        }

        throw new NullReferenceException("Message named form data is expected, but not found");
    }

    public async Task<MessageDto> ActiveMessage(string messageId)
    {
        var entity = await RetrieveMessage(messageId);

        entity.IsSent = true;
        await context.SaveChangesAsync();
        await AddNavigation(entity);
        return entity.ToDto();
    }

    public async Task<MessageDto> EditMessage(string caller, MessageModel message)
    {
        var entity = await RetrieveMessage(message.Id);

        if (!string.Equals(entity.SenderId, caller) || !string.IsNullOrEmpty(entity.ForwardId))
            throw new UnauthorizedAccessException("Access denied: You do not have permission to access this resource.");
        if (string.Equals(message.Text, entity.Text) || string.IsNullOrEmpty(message.Text))
        {
            await AddNavigation(entity);
            return entity.ToDto();
        }

        entity.Text = message.Text;
        entity.IsEdited = true;
        entity.ModifiedAt = DateTime.Now;
        var entityEntry = context.Messages.Update(entity);
        await context.SaveChangesAsync();
        await AddNavigation(entity);
        return entityEntry.Entity.ToDto();
    }

    public async Task<MessageDto> DeleteMessageForMyself(string caller, string messageId)
    {
        try
        {
            var entity = await RetrieveMessage(messageId);
            // Check if user and message in same chat 
            await context.ChatClients.FirstAsync(m =>
                string.Equals(m.ClientId, caller) && string.Equals(m.ChatId, entity.ChatId));

            if (string.Equals(entity.SenderId, caller))
            {
                entity.IsSenderDeleted = true;
            }
            else if (!entity.ChatId.StartsWith("g-") && !entity.ChatId.StartsWith("c-"))
            {
                entity.IsReceiverDeleted = true;
            }

            entity.ModifiedAt = DateTime.Now;
            await context.SaveChangesAsync();
            await AddNavigation(entity);
            return entity.ToDto();
        }
        catch (Exception e)
        {
            throw new UnauthorizedAccessException("Access denied: You cannot delete this message");
        }
    }

    public async Task<MessageDto> DeleteMessageForEveryone(string caller, string messageId)
    {
        try
        {
            var entity = await RetrieveMessage(messageId);
            // Check if user and message in same chat 
            await context.ChatClients.FirstAsync(m =>
                string.Equals(m.ClientId, caller) && string.Equals(m.ChatId, entity.ChatId));
            // if in DM not sender or receiver, or in GM not sender
            if ((entity.ChatId.StartsWith("g-") || entity.ChatId.StartsWith("c-")) &&
                !string.Equals(entity.SenderId, caller)) throw new Exception();


            entity.IsReceiverDeleted = true;
            entity.IsSenderDeleted = true;
            entity.ModifiedAt = DateTime.Now;
            await context.SaveChangesAsync();
            await AddNavigation(entity);
            return entity.ToDto();
        }
        catch (Exception e)
        {
            throw new UnauthorizedAccessException("Access denied: You cannot delete this message");
        }
    }

    // @Deprecated
    // public async Task<IEnumerable<MessageModel>> GetMessages(string caller)
    // {
    //     var userGroups = await context.UserGroup.AsNoTracking().Where(item => string.Equals(item.UserId, caller))
    //         .Select(m => m.GroupId).ToListAsync();
    //
    //     var messages = await context.Messages.AsNoTracking()
    //         .Include(m => m.Sender)
    //         .Include(m => m.Files)
    //         .Include(m => m.ForwardedMessage)
    //         .Include(m => m.RepliedMessage)
    //         .Where(item => string.Equals(caller, item.SenderId) && !item.IsSenderDeleted)
    //         .Where(item => item.IsSent)
    //         .OrderBy(item => item.CreatedAt)
    //         .ToListAsync();
    //
    //     foreach (var message in messages) await IncludeFiles(message);
    //
    //     return messages;
    // }

    public async Task<List<ChatDto>> GetChats(string caller)
    {
        var userChats = await context.ChatClients
            .Where(cu => string.Equals(cu.ClientId, caller))
            .Select(cu => new
            {
                ChatId = cu.ChatId,
                CompanionId = context.ChatClients
                    .Where(m => string.Equals(m.ChatId, cu.ChatId) && !string.Equals(m.ClientId, caller))
                    .Select(m => m.ClientId).FirstOrDefault(),
                LastMessage = cu.Chat.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault()
            })
            .ToListAsync();
        foreach (var chat in userChats)
        {
            await context.Entry(chat.LastMessage).Reference(m => m.Sender).LoadAsync();
        }

        return userChats.Select(m => new ChatDto
        {
            ChatId = m.ChatId,
            Message = m.LastMessage.Text,
            MessageTime = m.LastMessage.CreatedAt,
            ChatName = m.CompanionId != null && m.CompanionId.StartsWith("g-")
                ? context.Groups.Where(gm => string.Equals(gm.Id, m.CompanionId)).Select(m => m.Name).FirstOrDefault()
                : context.Users.Where(gm => string.Equals(gm.Id, m.CompanionId))
                    .Select(m => m.FirstName + " " + m.LastName).FirstOrDefault(),
            ReceiverId = m.CompanionId,
            SenderId = m.LastMessage.SenderId,
            SenderName = m.LastMessage.Sender.FirstName,
            IsForward = !m.LastMessage.ForwardId.IsNullOrEmpty()
        }).OrderBy(m => m.MessageTime).ToList();
    }

    public async Task<List<MessageDto>> GetMessagesByChatId(string caller, string chatId)
    {
        try
        {
            await context.ChatClients.FirstAsync(m => string.Equals(m.ClientId, caller) &&
                                                      string.Equals(m.ChatId, chatId));
            return await FetchMessagesByChatId(chatId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<MessageDto>> GetMessagesByUserId(string caller, string userId)
    {
        try
        {
            var commonChatId = await GetCommonChatId(caller, userId);
            if (string.IsNullOrEmpty(commonChatId)) return [];

            return await FetchMessagesByChatId(commonChatId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<MessageDto> PinMessage(string messageId)
    {
        var entity = await RetrieveMessage(messageId);
        entity.IsPinned = !entity.IsPinned;
        await context.SaveChangesAsync();
        await AddNavigation(entity);
        return entity.ToDto();
    }

    private async Task AddNavigation(MessageModel entity)
    {
        await context.Entry(entity).Reference(m => m.Sender).LoadAsync();
        await context.Entry(entity).Reference(m => m.RepliedMessage).LoadAsync();
        await context.Entry(entity).Reference(m => m.Chat).LoadAsync();
        await context.Entry(entity.Chat).Collection(m => m.ChatUsers).LoadAsync();
        await context.Entry(entity).Collection(m => m.Files).LoadAsync();
        await context.Entry(entity).Reference(m => m.ForwardedMessage).LoadAsync();
        await IncludeFiles(entity);
        if (entity.ReceiverId.StartsWith("g-"))
        {
            entity.Receiver = await context.Groups.Where(m => string.Equals(m.Id, entity.ReceiverId))
                .Select(m => m.Name).FirstOrDefaultAsync();
        }
        else
        {
            entity.Receiver = await context.Users.Where(m => string.Equals(m.Id, entity.ReceiverId))
                .Select(m => m.FirstName + " " + m.LastName).FirstOrDefaultAsync();
        }
    }

    private async Task<MessageModel> RetrieveMessage(string messageId)
    {
        return await context.Messages.FirstAsync(m => string.Equals(m.Id, messageId));
    }

    private async Task<List<MessageDto>> FetchMessagesByChatId(string chatId)
    {
        var chatModels = await context.Chats.Where(m => string.Equals(m.Id, chatId))
            .Include(m => m.Messages)
            .Select(m => m.Messages)
            .FirstAsync();

        foreach (var model in chatModels)
        {
            await AddNavigation(model);
        }

        var messageDtos = chatModels.Select(m => m.ToDto()).ToList();

        return messageDtos;
    }

    private async Task IncludeFiles(MessageModel? message)
    {
        if (message?.ForwardedMessage != null)
            await AddNavigation(message.ForwardedMessage);

        if (message?.RepliedMessage != null)
            await AddNavigation(message.RepliedMessage);
    }

    private async Task<MessageDto> SaveMessage(MessageRequest message, string senderId, bool isHub = true)
    {
        if (isHub && string.IsNullOrWhiteSpace(message.Text) && string.IsNullOrWhiteSpace(message.ForwardId))
        {
            return new MessageDto();
        }

        message = await ProvideChatId(senderId, message);

        // Parse nested forward and get the origin forwarded message
        // If you forward forwarded message => origin message will be forwarded
        if (!string.IsNullOrEmpty(message.ForwardId))
        {
            var forward = message.ForwardId;
            do
            {
                var nestedForward = await context.Messages.FirstAsync(m => string.Equals(m.Id, forward));
                if (string.IsNullOrEmpty(nestedForward.ForwardId)) break;
                forward = nestedForward.ForwardId;
            } while (true);

            message.ForwardId = forward;
        }


        var entity = message.ToMessageModel(senderId);
        var entityEntry = context.Messages.Add(entity);
        await context.SaveChangesAsync();
        await AddNavigation(entityEntry.Entity);
        return entityEntry.Entity.ToDto();
    }

    private async Task<MessageRequest> ProvideChatId(string senderId, MessageRequest message)
    {
        if (!string.IsNullOrWhiteSpace(message.ChatId) || string.IsNullOrWhiteSpace(message.ReceiverId)) return message;
        var commonChatId = await GetCommonChatId(senderId, message.ReceiverId);
        if (!string.IsNullOrEmpty(commonChatId))
        {
            if (message.ReceiverId.StartsWith("g-"))
            {
                var chatClientExists = await context.ChatClients.FirstOrDefaultAsync(m =>
                    string.Equals(m.ChatId, commonChatId) && string.Equals(m.ClientId, senderId));
                if (chatClientExists == null)
                {
                    context.ChatClients.Add(new ChatClientModel { ChatId = commonChatId, ClientId = senderId });
                }
            }

            message.ChatId = commonChatId;
            return message;
        }
        else
        {
            var id =
                message.ReceiverId.StartsWith("g-") ? message.ReceiverId : Guid.NewGuid().ToString();

            var entry = context.Chats.Add(new ChatModel { Id = id });
            context.ChatClients.Add(new ChatClientModel { ChatId = entry.Entity.Id, ClientId = senderId });
            context.ChatClients.Add(new ChatClientModel
                { ChatId = entry.Entity.Id, ClientId = message.ReceiverId });
            message.ChatId = entry.Entity.Id;
            return message;
        }
    }

    private async Task<string?> GetCommonChatId(string senderId, string receiverId)
    {
        if (receiverId.StartsWith("g-"))
        {
            return await context.Chats
                .Where(m => string.Equals(m.Id, receiverId)).Select(m => m.Id)
                .FirstOrDefaultAsync();
        }

        return await context.Chats
            .Where(chat => chat.ChatUsers.Any(cu => cu.ClientId == senderId) &&
                           chat.ChatUsers.Any(cu => cu.ClientId == receiverId))
            .Select(m => m.Id)
            .FirstOrDefaultAsync();
    }
}