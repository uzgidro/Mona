using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mona.Model;

namespace Mona.Context;

public class ApplicationContext(DbContextOptions<ApplicationContext> options)
    : IdentityDbContext<UserModel>(options)
{
    public DbSet<MessageModel> Messages => Set<MessageModel>();

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
    }
}