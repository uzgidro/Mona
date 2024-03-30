namespace Mona.Model;

public class FileModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string Path { get; set; }
    public string MessageId { get; set; }
}