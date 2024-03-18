using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mona.Model;

namespace Mona.Context;

public class ApplicationContext(DbContextOptions<ApplicationContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<MessageItem> Messages => Set<MessageItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<MessageItem>()
            .HasOne(m => m.Receiver)
            .WithMany(u => u.ReceivedMessages)
            .HasForeignKey(m => m.ReceiverId)
            .IsRequired();

        builder.Entity<MessageItem>()
            .HasOne(m => m.Sender)
            .WithMany(u => u.SentMessages)
            .HasForeignKey(m => m.SenderId)
            .IsRequired();
    }
}