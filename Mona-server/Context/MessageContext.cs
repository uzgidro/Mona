using Microsoft.EntityFrameworkCore;
using Mona.Model;

namespace Mona.Context;

public class MessageContext(DbContextOptions<MessageContext> options) : DbContext(options)
{
    public DbSet<MessageItem> Messages => Set<MessageItem>();
}