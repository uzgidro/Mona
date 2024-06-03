namespace Mona.Model.Dto;

public struct ChatResponse
{
    public string ChatId { get; init; }
    public string ChatName { get; init; }
    public string Message { get; init; }
    public DateTime MessageTime { get; init; }
    public string? SenderId { get; init; }
    public string? ChatIcon { get; init; }
}