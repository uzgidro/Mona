using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Mona.Context;
using Mona.Model;
using Mona.Model.Dto;
using Mona.Service.Interface;
using Mona.Utilities;

namespace Mona.Service;

public class MessageService(ApplicationContext context) : IMessageService
{
    private static readonly JsonSerializerOptions? Options = new() { PropertyNameCaseInsensitive = true };

    public async Task<MessageModel> CreateMessage(MessageModel message)
    {
        if (string.IsNullOrEmpty(message.Text)) return new MessageModel();
        var entityEntry = context.Messages.Add(message);
        await context.SaveChangesAsync();
        await AddNavigation(entityEntry.Entity);
        return entityEntry.Entity;
    }

    public async Task<MessageModel> CreateMessage(MultipartReader multipartReader, string senderId)
    {
        var section = await multipartReader.ReadNextSectionAsync();
        // var userChats = await context.ChatClients
        //     .Where(m => string.Equals(m.ChatId, senderId))
        //     .Include(m => m.Chat)
        //     .Select(m => m.Chat)
        //     .ToListAsync();

        while (section != null)
        {
            if (section.Headers != null && section.Headers.ContainsValue("form-data; name=\"message\""))
            {
                using var reader = new StreamReader(section.Body, Encoding.UTF8);
                var messageJson = await reader.ReadToEndAsync();
                var message = JsonSerializer.Deserialize<MessageRequest>(messageJson, Options);

                if (string.IsNullOrEmpty(message.ChatId))
                {
                    var commonChat = await context.Chats
                        .Where(chat => chat.ChatUsers.Any(cu => cu.ClientId == senderId) &&
                                       chat.ChatUsers.Any(cu => cu.ClientId == message.ReceiverId))
                        .FirstOrDefaultAsync();
                    if (commonChat != null)
                    {
                        message.ChatId = commonChat.Id;
                    }
                    else
                    {
                        var entry = context.Chats.Add(new ChatModel());
                        context.ChatClients.Add(new ChatClientModel()
                            { ChatId = entry.Entity.Id, ClientId = senderId });
                        context.ChatClients.Add(new ChatClientModel()
                            { ChatId = entry.Entity.Id, ClientId = message.ReceiverId });
                        message.ChatId = entry.Entity.Id;
                    }
                }

                var entity = message.ToMessageModel(senderId);
                var entityEntry = context.Messages.Add(entity);
                await context.SaveChangesAsync();
                return entityEntry.Entity;
            }

            section = await multipartReader.ReadNextSectionAsync();
        }

        throw new NullReferenceException("Message named form data is expected, but not found");
    }

    public async Task<MessageModel> ActiveMessage(string messageId)
    {
        var entity = await RetrieveMessage(messageId);

        entity.IsSent = true;
        await context.SaveChangesAsync();
        await AddNavigation(entity);
        return entity;
    }

    public async Task<MessageModel> EditMessage(string caller, MessageModel message)
    {
        var entity = await RetrieveMessage(message.Id);

        if (!string.Equals(entity.SenderId, caller) || !string.IsNullOrEmpty(entity.ForwardId))
            throw new UnauthorizedAccessException("Access denied: You do not have permission to access this resource.");
        if (string.Equals(message.Text, entity.Text) || string.IsNullOrEmpty(message.Text))
        {
            await AddNavigation(entity);
            return entity;
        }

        entity.Text = message.Text;
        entity.IsEdited = true;
        entity.ModifiedAt = DateTime.Now;
        var entityEntry = context.Messages.Update(entity);
        await context.SaveChangesAsync();
        await AddNavigation(entity);
        return entityEntry.Entity;
    }

    public async Task<MessageModel> DeleteMessageForMyself(string caller, string messageId)
    {
        var entity = await RetrieveMessage(messageId);

        if (string.Equals(entity.SenderId, caller))
        {
            entity.IsSenderDeleted = true;
        }
        else if (string.Equals(entity.DirectReceiverId, caller))
        {
            entity.IsReceiverDeleted = true;
        }
        // if receiver not found or receiver type is group model
        else throw new UnauthorizedAccessException("Access denied: You cannot delete this message");

        entity.ModifiedAt = DateTime.Now;
        await context.SaveChangesAsync();
        await AddNavigation(entity);
        return entity;
    }

    public async Task<MessageModel> DeleteMessageForEveryone(string caller, string messageId)
    {
        var entity = await RetrieveMessage(messageId);
        // if in DM not sender or receiver, or in GM not sender
        if ((string.IsNullOrEmpty(entity.DirectReceiverId) ||
             (!string.Equals(entity.SenderId, caller) &&
              !string.Equals(entity.DirectReceiverId, caller))) &&
            (string.IsNullOrEmpty(entity.GroupReceiverId) ||
             !string.Equals(entity.SenderId, caller)))
            throw new UnauthorizedAccessException("You cannot delete this message");

        entity.IsReceiverDeleted = true;
        entity.IsSenderDeleted = true;
        entity.ModifiedAt = DateTime.Now;
        await context.SaveChangesAsync();
        await AddNavigation(entity);
        return entity;
    }

    public async Task<IEnumerable<MessageModel>> GetMessages(string caller)
    {
        var userGroups = await context.UserGroup.AsNoTracking().Where(item => string.Equals(item.UserId, caller))
            .Select(m => m.GroupId).ToListAsync();

        var messages = await context.Messages.AsNoTracking()
            .Include(m => m.Sender)
            .Include(m => m.DirectReceiver)
            .Include(m => m.GroupReceiver)
            .Include(m => m.Files)
            .Include(m => m.ForwardedMessage)
            .Include(m => m.RepliedMessage)
            .Where(item =>
                (string.Equals(caller, item.DirectReceiverId) && !item.IsReceiverDeleted) ||
                (string.Equals(caller, item.SenderId) && !item.IsSenderDeleted) ||
                userGroups.Contains(item.GroupReceiverId))
            .Where(item => item.IsSent)
            .OrderBy(item => item.CreatedAt)
            .ToListAsync();

        foreach (var message in messages) await IncludeFiles(message);

        return messages;
    }

    public async Task<List<ChatResponse>> GetChats(string caller)
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
        return userChats.Select(m => new ChatResponse
        {
            ChatId = m.ChatId,
            Message = m.LastMessage.Text,
            MessageTime = m.LastMessage.CreatedAt,
            ChatName = m.CompanionId != null && m.CompanionId.StartsWith("g-")
                ? context.Groups.Where(gm => string.Equals(gm.Id, m.CompanionId)).Select(m => m.Name).FirstOrDefault()
                : context.Users.Where(gm => string.Equals(gm.Id, m.CompanionId))
                    .Select(m => m.FirstName + " " + m.LastName).FirstOrDefault(),
        }).OrderBy(m => m.MessageTime).ToList();
    }

    public async Task<List<MessageDto>> GetChatMessages(string caller, string chatId)
    {
        try
        {
            await context.ChatClients.FirstAsync(m => string.Equals(m.ClientId, caller) &&
                                                      string.Equals(m.ChatId, chatId));
            var chatModels = await context.Chats.Where(m => string.Equals(m.Id, chatId))
                .Include(m => m.Messages)
                .Select(m => m.Messages)
                .FirstAsync();

            foreach (var model in chatModels)
            {
                await AddNavigation(model);
            }

            var messageDtos = chatModels.Select(m => m.ToDto()).ToList();


            // .Select(m => m.Messages.Select(n => n.ToDto())).FirstAsync();
            Console.WriteLine(messageDtos);
            return messageDtos;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<MessageModel> PinMessage(string messageId)
    {
        var entity = await RetrieveMessage(messageId);
        entity.IsPinned = !entity.IsPinned;
        await context.SaveChangesAsync();
        await AddNavigation(entity);
        return entity;
    }

    private async Task AddNavigation(MessageModel entity)
    {
        await context.Entry(entity).Reference(m => m.Sender).LoadAsync();
        await context.Entry(entity).Reference(m => m.DirectReceiver).LoadAsync();
        await context.Entry(entity).Reference(m => m.GroupReceiver).LoadAsync();
        await context.Entry(entity).Reference(m => m.RepliedMessage).LoadAsync();
        await context.Entry(entity).Reference(m => m.Chat).LoadAsync();
        await context.Entry(entity).Collection(m => m.Files).LoadAsync();
        await context.Entry(entity).Reference(m => m.ForwardedMessage).LoadAsync();
        await IncludeFiles(entity);
    }

    private async Task<MessageModel> RetrieveMessage(string messageId)
    {
        return await context.Messages.FirstAsync(m => string.Equals(m.Id, messageId));
    }

    private async Task IncludeFiles(MessageModel? message)
    {
        if (message?.ForwardedMessage != null)
            await AddNavigation(message.ForwardedMessage);

        if (message?.RepliedMessage != null)
            await AddNavigation(message.RepliedMessage);
    }
}