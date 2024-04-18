using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mona.Model;

namespace Mona.Context;

public class ApplicationContext(DbContextOptions<ApplicationContext> options)
    : IdentityDbContext<UserModel>(options)
{
    public DbSet<MessageModel> Messages => Set<MessageModel>();
    public DbSet<FileModel> Files => Set<FileModel>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<MessageModel>()
            .HasOne(m => m.Receiver)
            .WithMany()
            .HasForeignKey(m => m.ReceiverId)
            .IsRequired();

        builder.Entity<MessageModel>()
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId)
            .IsRequired();

        builder.Entity<FileModel>()
            .HasOne<MessageModel>()
            .WithMany(e => e.Files)
            .HasForeignKey(e => e.MessageId)
            .IsRequired();

        builder.Entity<MessageModel>()
            .HasOne(e => e.RepliedMessage)
            .WithOne()
            .HasForeignKey<MessageModel>(e => e.ReplyId)
            .IsRequired(false);

        builder.Entity<MessageModel>()
            .HasOne(e => e.ForwardedMessage)
            .WithOne()
            .HasForeignKey<MessageModel>(e => e.ForwardId)
            .IsRequired(false);
    }
}