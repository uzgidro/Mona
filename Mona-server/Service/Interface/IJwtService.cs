using System.Security.Claims;
using Mona.Model;

namespace Mona.Service.Interface;

public interface IJwtService
{
    string EncodeToken(User user);
    string EncodeRefreshToken(User user);
    ClaimsPrincipal? DecodeToken(string token);
    bool ValidToken(string token);
}