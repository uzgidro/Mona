using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Mona.Model;

namespace Mona.Service.Interface;

public interface IJwtService
{
    string EncodeToken(ApplicationUser applicationUser);
    string EncodeRefreshToken(ApplicationUser applicationUser);
    TokenValidationParameters GetValidationParameters();
    ClaimsPrincipal? DecodeToken(string token);
    bool ValidToken(string token);
}