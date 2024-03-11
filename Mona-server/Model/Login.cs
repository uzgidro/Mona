namespace Mona.Model;

public record Login
{
    public string PersonalId { get; set; }
    public string Password { get; set; }
}