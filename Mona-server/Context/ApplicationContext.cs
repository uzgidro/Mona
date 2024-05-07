using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mona.Model;

namespace Mona.Context;

public class ApplicationContext(DbContextOptions<ApplicationContext> options)
    : IdentityDbContext<UserModel>(options)
{
    public DbSet<MessageModel> Messages => Set<MessageModel>();
    public DbSet<GroupModel> Groups => Set<GroupModel>();
    public DbSet<UserGroup> UserGroup => Set<UserGroup>();
    public DbSet<FileModel> Files => Set<FileModel>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<MessageModel>()
            .HasOne(m => m.DirectReceiver)
            .WithMany()
            .HasForeignKey(m => m.DirectReceiverId)
            .IsRequired(false);

        builder.Entity<MessageModel>()
            .HasOne(m => m.GroupReceiver)
            .WithMany()
            .HasForeignKey(m => m.GroupReceiverId)
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
            .WithMany()
            .HasForeignKey(e => e.ReplyId)
            .IsRequired(false);

        builder.Entity<MessageModel>()
            .HasOne(e => e.ForwardedMessage)
            .WithMany()
            .HasForeignKey(e => e.ForwardId)
            .IsRequired(false);

        builder.Entity<MessageModel>(entity =>
        {
            entity.HasIndex(e => e.ForwardId);
            entity.HasIndex(e => e.ReplyId);
        });

        builder.Entity<UserModel>()
            .HasMany(e => e.Groups)
            .WithMany(e => e.Users)
            .UsingEntity<UserGroup>(
                l => l.HasOne<GroupModel>(e => e.GroupModel).WithMany(e => e.UserGroups).HasForeignKey(e => e.GroupId),
                r => r.HasOne<UserModel>(e => e.UserModel).WithMany(e => e.UserGroups).HasForeignKey(e => e.UserId));
    }
}