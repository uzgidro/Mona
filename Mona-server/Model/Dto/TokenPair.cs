namespace Mona.Model.Dto;

public struct TokenPair(string accessToken, string refreshToken)
{
    public string AccessToken { get; set; } = accessToken;
    public string RefreshToken { get; set; } = refreshToken;
}