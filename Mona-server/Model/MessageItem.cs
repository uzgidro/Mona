﻿namespace Mona.Model;

public class MessageItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Text { get; set; }
    public string SenderId { get; set; }
    public ApplicationUser Sender { get; set; }
    public string ReceiverId { get; set; }
    public ApplicationUser Receiver { get; set; }
    public string? ReplyId { get; set; }
    public bool IsEdited { get; set; }
    public bool IsForwarded { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}