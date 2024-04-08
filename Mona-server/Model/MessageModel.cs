namespace Mona.Model;

public class MessageModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Text { get; set; }
    public string SenderId { get; set; }
    public UserModel Sender { get; set; }
    public string ReceiverId { get; set; }
    public UserModel Receiver { get; set; }
    public string? ReplyId { get; set; }
    public ICollection<FileModel> Files { get; set; } = new List<FileModel>();
    public bool IsSent { get; set; }
    public bool IsFileOnly { get; set; }
    public bool IsEdited { get; set; }
    public bool IsForwarded { get; set; }
    public bool IsSenderDeleted { get; set; }
    public bool IsReceiverDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}