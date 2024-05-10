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

        while (section != null)
        {
            if (section.Headers != null && section.Headers.ContainsValue("form-data; name=\"message\""))
            {
                using var reader = new StreamReader(section.Body, Encoding.UTF8);
                var messageJson = await reader.ReadToEndAsync();
                var message = JsonSerializer.Deserialize<MessageRequest>(messageJson, Options);

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
        var entity = context.Messages.First(item =>
            string.Equals(item.Id, messageId)
            && string.Equals(item.SenderId, caller)
            && !string.Equals(item.DirectReceiverId, caller));

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
            await context.Entry(message.ForwardedMessage).Collection(m => m.Files).LoadAsync();

        if (message?.RepliedMessage != null)
            await context.Entry(message.RepliedMessage).Collection(m => m.Files).LoadAsync();
    }
}