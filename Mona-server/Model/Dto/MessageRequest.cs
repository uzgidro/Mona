namespace Mona.Model.Dto;

public struct MessageRequest
{
    public string Text { get; set; }
    public string ReceiverId { get; set; }
    public string? ReplyId { get; set; }
    public string? ForwardId { get; set; }
    public DateTime CreatedAt { get; set; }
}