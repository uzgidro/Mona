namespace Mona.Model.Dto;

public struct CustomRegisterRequest
{
    public string Username { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Password { get; init; }
}