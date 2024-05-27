namespace Mona.Model;

public class ChatModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public ICollection<MessageModel> Messages { get; set; }
    public ICollection<ChatClientModel> ChatUsers { get; set; }
}