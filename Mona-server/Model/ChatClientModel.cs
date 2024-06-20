namespace Mona.Model;

public class ChatClientModel
{
    public string ClientId { get; set; }
    public UserModel Client { get; set; }
    public string ChatId { get; set; }
    public ChatModel Chat { get; set; }
}