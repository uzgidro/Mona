using Microsoft.EntityFrameworkCore;
using Mona.Context;
using Mona.Model;
using Mona.Model.Dto;
using Mona.Service.Interface;

namespace Mona.Service;

public class MessageService(ApplicationContext context) : IMessageService
{
    public async Task<MessageItem> CreateMessage(MessageRequest message)
    {
        if (string.IsNullOrEmpty(message.Text)) return new MessageItem();
        var entity = new MessageItem
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
        return entityEntry.Entity;
    }

    public async Task<IEnumerable<MessageItem>> GetMessages(string caller)
    {
        return await context.Messages.AsNoTracking()
            .Where(item => !item.IsDeleted && (item.ReceiverId == caller || item.SenderId == caller))
            .ToListAsync();
    }
}