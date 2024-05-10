namespace Mona.Model;

public class MessageModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Text { get; set; }
    public string SenderId { get; set; }
    public UserModel Sender { get; set; }
    public string? DirectReceiverId { get; set; }
    public UserModel? DirectReceiver { get; set; }
    public string? GroupReceiverId { get; set; }
    public GroupModel? GroupReceiver { get; set; }
    public string? ReplyId { get; set; }
    public MessageModel? RepliedMessage { get; set; }
    public string? ForwardId { get; set; }
    public MessageModel? ForwardedMessage { get; set; }
    public ICollection<FileModel> Files { get; set; } = new List<FileModel>();
    public bool IsPinned { get; set; }
    public bool IsSent { get; set; }
    public bool IsEdited { get; set; }
    public bool IsSenderDeleted { get; set; }
    public bool IsReceiverDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}