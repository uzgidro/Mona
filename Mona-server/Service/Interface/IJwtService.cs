using Microsoft.IdentityModel.Tokens;
using Mona.Model;

namespace Mona.Service.Interface;

public interface IJwtService
{
    TokenPair EncodeTokenPair(ApplicationUser applicationUser);
    string RefreshTokens(TokenPair tokens);
    TokenValidationParameters GetValidationParameters(bool validateLifetime = true);
}