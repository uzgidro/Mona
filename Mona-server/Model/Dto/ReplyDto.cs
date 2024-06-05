namespace Mona.Model.Dto;

public struct ReplyDto
{
    public string Id { get; init; }
    public string ReplyTo { get; init; }
    public string RepliedMessage { get; init; }
}