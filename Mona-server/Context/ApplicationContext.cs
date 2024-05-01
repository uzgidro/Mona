using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mona.Model;

namespace Mona.Context;

public class ApplicationContext(DbContextOptions<ApplicationContext> options)
    : IdentityDbContext<UserModel>(options)
{
    public DbSet<MessageModel> Messages => Set<MessageModel>();
    public DbSet<GroupModel> Groups => Set<GroupModel>();
    public DbSet<FileModel> Files => Set<FileModel>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<MessageModel>()
            .HasOne(m => m.UserReceiver)
            .WithMany()
            .HasForeignKey(m => m.ReceiverId)
            .IsRequired(false);

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

        builder.Entity<UserModel>()
            .HasMany(e => e.Groups)
            .WithMany(e => e.Users)
            .UsingEntity<UserGroup>(
                l => l.HasOne<GroupModel>().WithMany().HasForeignKey(e => e.GroupId),
                r => r.HasOne<UserModel>().WithMany().HasForeignKey(e => e.UserId));
    }
}