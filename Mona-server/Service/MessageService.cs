using Microsoft.EntityFrameworkCore;
using Mona.Context;
using Mona.Model;

namespace Mona.Service;

public class MessageService(MessageContext context) : IMessageService
{
    public async void CreateMessage(MessageItem message)
    {
        if (string.IsNullOrEmpty(message.Text) && string.IsNullOrEmpty(message.Group)) return;
        context.Messages.Add(message);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<MessageItem>> GetMessages()
    {
       return await context.Messages.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<MessageItem>> GetMessagesByGroup(string group)
    {
        return await context.Messages.AsNoTracking().Where(item => item.Group.Equals(group)).ToListAsync();
    }
}