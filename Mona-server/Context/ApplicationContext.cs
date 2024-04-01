using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mona.Model;
using System.Reflection.Emit;
using System;

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
            .WithMany()
            .HasForeignKey(m => m.ReceiverId)
            .IsRequired();

        builder.Entity<MessageItem>()
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId)
            .IsRequired();

        builder.Entity<MessageItem>()
            .HasOne(e => e.RepliedMessage)
            .WithOne()
            .HasForeignKey<MessageItem>(e => e.ReplyId)
            .IsRequired(false);
    }
}