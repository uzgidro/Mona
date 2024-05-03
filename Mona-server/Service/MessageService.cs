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

    // public async Task<MessageModel> CreateMessage(MessageRequest message)
    // {
    //     if (string.IsNullOrEmpty(message.Text)) return new MessageModel();
    //     var entity = new MessageModel
    //     {
    //         Text = message.Text,
    //         SenderId = message.SenderId!,
    //         ReceiverId = message.ReceiverId,
    //         CreatedAt = message.CreatedAt,
    //         ModifiedAt = message.CreatedAt,
    //         ReplyId = message.ReplyId
    //     };
    //     var entityEntry = context.Messages.Add(entity);
    //     await context.SaveChangesAsync();
    //     await AddNavigation(entity);
    //     return entityEntry.Entity;
    // }

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

    public async Task<MessageModel> ActiveMessage(MessageModel messageModel)
    {
        var entity = context.Messages
            .First(item => item.Id.Equals(messageModel.Id));

        entity.IsSent = true;
        await context.SaveChangesAsync();
        await AddNavigation(entity);
        return entity;
    }

    public async Task<MessageModel> EditMessage(string? caller, MessageModel message)
    {
        var entity = context.Messages.First(item => item.Id == message.Id);

        if (!string.Equals(entity.SenderId, caller) || !string.IsNullOrEmpty(entity.ForwardId))
            throw new UnauthorizedAccessException("Access denied: You do not have permission to access this resource.");
        if (string.Equals(message.Text, entity.Text) || string.IsNullOrEmpty(message.Text))
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

    public async Task<MessageModel> DeleteMessageForMyself(string? caller, MessageModel message)
    {
        var entity = context.Messages.First(item => item.Id == message.Id);

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

    public async Task<MessageModel> DeleteMessageForEveryone(string? caller, MessageModel message)
    {
        var entity = context.Messages.First(item =>
            string.Equals(item.Id, message.Id)
            && string.Equals(item.SenderId, caller)
            && !string.Equals(item.DirectReceiverId, caller));

        entity.IsReceiverDeleted = true;
        entity.IsSenderDeleted = true;
        entity.ModifiedAt = DateTime.Now;
        await context.SaveChangesAsync();
        await AddNavigation(entity);
        return entity;
    }

    public async Task<IEnumerable<MessageModel>> GetMessages(string? caller)
    {
        return await context.Messages.AsNoTracking()
            .Include(m => m.Sender)
            .Include(m => m.UserReceiver)
            .Include(m => m.GroupReceiver)
            .Include(m => m.Files)
            .Include(m => m.ForwardedMessage)
            .Where(item => (string.Equals(caller, item.DirectReceiverId) && !item.IsReceiverDeleted) ||
                           (string.Equals(caller, item.SenderId) && !item.IsSenderDeleted))
            .Where(item => item.IsSent)
            .OrderBy(item => item.CreatedAt)
            .ToListAsync();
    }

    private async Task AddNavigation(MessageModel? entity)
    {
        await context.Entry(entity).Reference(m => m.Sender).LoadAsync();
        await context.Entry(entity).Reference(m => m.UserReceiver).LoadAsync();
        await context.Entry(entity).Reference(m => m.GroupReceiver).LoadAsync();
        await context.Entry(entity).Reference(m => m.RepliedMessage).LoadAsync();
        await context.Entry(entity).Collection(m => m.Files).LoadAsync();
        await context.Entry(entity).Reference(m => m.ForwardedMessage).LoadAsync();
    }
}