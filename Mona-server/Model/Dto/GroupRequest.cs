namespace Mona.Model.Dto;

public struct GroupRequest
{
    public string Name { get; init; }
    public string? Description { get; init; }
    public string? CreatorId { get; set; }
    public IEnumerable<string>? Members { get; init; }
}