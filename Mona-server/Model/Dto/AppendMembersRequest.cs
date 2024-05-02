namespace Mona.Model.Dto;

public class AppendMembersRequest
{
    public required IEnumerable<string> Members { get; init; }
}