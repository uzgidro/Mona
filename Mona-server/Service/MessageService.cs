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
        await AddNavigation(entity);
        return entityEntry.Entity;
    }

    public async Task<MessageItem?> EditMessage(MessageItem message)
    {
        var entity = await context.Messages.FirstOrDefaultAsync(item => item.Id == message.Id);

        if (entity == null) return null;
        if (entity.Text == message.Text || string.IsNullOrEmpty(message.Text))
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

    public async Task<IEnumerable<MessageItem>> GetMessages(string caller)
    {
        return await context.Messages.AsNoTracking()
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(item => !item.IsDeleted && (item.ReceiverId == caller || item.SenderId == caller))
            .ToListAsync();
    }

    private async Task AddNavigation(MessageItem? entity)
    {
        await context.Entry(entity).Reference(m => m.Sender).LoadAsync();
        await context.Entry(entity).Reference(m => m.Receiver).LoadAsync();
    }
}