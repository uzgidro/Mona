namespace Mona.Model;

public struct RegisterRequest
{
    public string UserName { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Password { get; init; }
}