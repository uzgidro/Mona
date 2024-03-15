namespace Mona.Hub;

public interface IHubInterfaces
{
    Task ReceiveMessage(string sender, string message);
}