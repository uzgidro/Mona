namespace Mona.Model.Dto;

public struct ChatResponse
{
    public String ChatId { get; init; }
    public String ChatName { get; init; }
    public String Message { get; init; }
    public DateTime MessageTime { get; init; }
    public String? SenderId { get; init; }
    public String? ChatIcon { get; init; }
}