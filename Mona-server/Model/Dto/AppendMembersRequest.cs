namespace Mona.Model.Dto;

public struct AppendMembersRequest
{
    public required IEnumerable<string> Members { get; init; }
}