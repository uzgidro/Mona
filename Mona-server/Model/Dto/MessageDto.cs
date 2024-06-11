namespace Mona.Model.Dto;

public struct MessageDto
{
    public string Id { get; init; }
    public string SenderId { get; init; }
    public string SenderName { get; init; }
    public string ChatId { get; init; }
    public string ReceiverId { get; init; }
    public string? Message { get; init; }
    public IEnumerable<FileDto>? Files { get; init; }
    public ForwardDto? Forward { get; init; }
    public ReplyDto? Reply { get; init; }
    public bool IsPinned { get; init; }
    public bool IsEdited { get; init; }
    public DateTime CreatedAt { get; init; }
}