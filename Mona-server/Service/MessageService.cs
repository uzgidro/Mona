using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Mona.Context;
using Mona.Model;
using Mona.Model.Dto;
using Mona.Service.Interface;

namespace Mona.Service;

public class MessageService(ApplicationContext context) : IMessageService
{
    private static readonly JsonSerializerOptions? Options = new JsonSerializerOptions
        { PropertyNameCaseInsensitive = true };

    public async Task<MessageModel> CreateMessage(MessageRequest message)
    {
        if (string.IsNullOrEmpty(message.Text)) return new MessageModel();
        var entity = new MessageModel
        {
            Text = message.Text,
            SenderId = message.SenderId!,
            ReceiverId = message.ReceiverId,
            CreatedAt = message.CreatedAt,
            ModifiedAt = message.CreatedAt,
            ReplyId = message.ReplyId
        };
        var entityEntry = context.Messages.Add(entity);
        await context.SaveChangesAsync();
        await AddNavigation(entity);
        return entityEntry.Entity;
    }

    public async Task<MessageModel?> CreateMessage(MultipartReader multipartReader, string senderId)
    {
        var section = await multipartReader.ReadNextSectionAsync();

        while (section != null)
        {
            if (section.Headers.ContainsValue("form-data; name=\"message\""))
            {
                using var reader = new StreamReader(section.Body, Encoding.UTF8);
                var messageJson = await reader.ReadToEndAsync();
                var message = JsonSerializer.Deserialize<MessageRequest>(messageJson, Options);
                message.SenderId = senderId;
                var entity = new MessageModel
                {
                    Text = message.Text,
                    SenderId = message.SenderId!,
                    ReceiverId = message.ReceiverId,
                    CreatedAt = message.CreatedAt,
                    ModifiedAt = message.CreatedAt,
                    ReplyId = message.ReplyId,
                    IsSent = false
                };
                var entityEntry = context.Messages.Add(entity);
                await context.SaveChangesAsync();
                return entityEntry.Entity;
            }

            section = await multipartReader.ReadNextSectionAsync();
        }

        return null;
    }

    public async Task<MessageModel?> ActiveMessage(MessageModel messageModel)
    {
        var entity = await context.Messages
            .Where(item => item.Id.Equals(messageModel.Id))
            .FirstOrDefaultAsync();
        if (entity == null) return null;

        entity.IsSent = true;
        var entityEntry = context.Messages.Update(entity);
        await context.SaveChangesAsync();
        await AddNavigation(entity);
        return entityEntry.Entity;
    }

    public async Task<MessageModel?> EditMessage(string? caller, MessageModel message)
    {
        var entity = await context.Messages.FirstOrDefaultAsync(item => item.Id == message.Id);

        if (entity == null || !entity.SenderId.Equals(caller)) return null;
        if (entity.Text.Equals(message.Text) || string.IsNullOrEmpty(message.Text))
        {
            var entry = context.Messages.Entry(entity);
            await AddNavigation(entity);
            return entry.Entity;
        }

        entity.Text = message.Text;
        entity.IsEdited = true;
        entity.ModifiedAt = DateTime.Now;
        var entityEntry = context.Messages.Update(entity);
        await context.SaveChangesAsync();
        await AddNavigation(entity);
        return entityEntry.Entity;
    }

    public async Task<MessageModel?> DeleteMessageForMyself(string? caller, MessageModel message)
    {
        var entity = await context.Messages.FirstOrDefaultAsync(item => item.Id == message.Id);

        if (entity == null) return null;
        if (entity.SenderId.Equals(caller))
        {
            entity.IsSenderDeleted = true;
        }
        else if (entity.ReceiverId.Equals(caller))
        {
            entity.IsReceiverDeleted = true;
        }
        else return null;

        entity.ModifiedAt = DateTime.Now;
        var entityEntry = context.Messages.Update(entity);
        await context.SaveChangesAsync();
        await AddNavigation(entity);
        return entityEntry.Entity;
    }

    public async Task<MessageModel?> DeleteMessageForEveryone(string? caller, MessageModel message)
    {
        var entity = await context.Messages.FirstOrDefaultAsync(item => item.Id == message.Id);

        if (entity == null || (!entity.ReceiverId.Equals(caller) && !entity.SenderId.Equals(caller))) return null;
        entity.IsReceiverDeleted = true;
        entity.IsSenderDeleted = true;
        entity.ModifiedAt = DateTime.Now;
        var entityEntry = context.Messages.Update(entity);
        await context.SaveChangesAsync();
        await AddNavigation(entity);
        return entityEntry.Entity;
    }

    public async Task<IEnumerable<MessageModel>> GetMessages(string? caller)
    {
        return await context.Messages.AsNoTracking()
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(item => (item.ReceiverId.Equals(caller) && !item.IsReceiverDeleted) ||
                           (item.SenderId.Equals(caller) && !item.IsSenderDeleted))
            .Where(item => item.IsSent)
            .ToListAsync();
    }

    private async Task AddNavigation(MessageModel? entity)
    {
        await context.Entry(entity).Reference(m => m.Sender).LoadAsync();
        await context.Entry(entity).Reference(m => m.Receiver).LoadAsync();
        await context.Entry(entity).Collection(m => m.Files).LoadAsync();
    }
}