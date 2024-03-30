using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Mona.Enum;
using Mona.Model;
using Mona.Model.Dto;
using Mona.Service.Interface;

namespace Mona.Service;

public class JwtService(IConfiguration configuration) : IJwtService
{
    public TokenPair EncodeTokenPair(UserModel userModel)
    {
        var credentials = GetCredentials();
        var accessClaims = new ClaimsIdentity(new List<Claim>
            {
                new(Claims.Id, userModel.Id),
                new(Claims.Username, userModel.UserName),
                new(Claims.Role, "User"),
                new(Claims.FirstName, userModel.LastName),
                new(Claims.LastName, userModel.LastName),
            }, "Token", Claims.Username,
            Claims.Role);
        var access = new JwtSecurityToken(
            issuer: "Server",
            audience: "Client",
            claims: accessClaims.Claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: credentials
        );

        var refreshClaims = new ClaimsIdentity(new List<Claim>
            {
                new(Claims.Id, userModel.Id),
                new(Claims.Username, userModel.UserName),
                new(Claims.Role, "User"),
            }, "Token", Claims.Username,
            Claims.Role);
        var refresh = new JwtSecurityToken(
            issuer: "Server",
            audience: "Client",
            claims: refreshClaims.Claims,
            expires: DateTime.UtcNow.AddMonths(1),
            signingCredentials: credentials
        );

        return new TokenPair(
            new JwtSecurityTokenHandler().WriteToken(access),
            new JwtSecurityTokenHandler().WriteToken(refresh)
        );
    }

    public string RefreshTokens(TokenPair tokens)
    {
        try
        {
            var usernameFromAccess = DecodeToken(tokens.AccessToken, false).Claims
                .FirstOrDefault(claim => claim.Type.Equals(Claims.Username)).Value;
            var usernameFromRefresh = DecodeToken(tokens.RefreshToken).Claims
                .FirstOrDefault(claim => claim.Type.Equals(Claims.Username)).Value;
            if (usernameFromAccess == usernameFromRefresh)
            {
                return usernameFromAccess;
            }
        }
        catch
        {
            return "";
        }

        return "";
    }

    public TokenValidationParameters GetValidationParameters(bool validateLifetime = true)
    {
        var securityKey = GetPublicKey();
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = validateLifetime,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "Server",
            ValidAudiences = new List<string> { "Client", "Web", "Mobile" },
            IssuerSigningKey = securityKey,
        };
    }

    private ClaimsPrincipal? DecodeToken(string token, bool validateLifetime = true)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = GetValidationParameters(validateLifetime);
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.RsaSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");
        return principal;
    }

    private bool ValidToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();
            tokenHandler.ValidateToken(token, validationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private SigningCredentials GetCredentials()
    {
        var securityKey = GetPrivateKey();
        return new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);
    }

    private RsaSecurityKey GetPrivateKey()
    {
        var key = "-----BEGIN PRIVATE KEY-----MIICXQIBAAKBgQCiz9P/tdCfvRATD8CQVDMRoHuTW3WrLibnewj7DKD93GGOBfVQRrkTOz+FDo+Qs7NxNBGnFByrnFgf2xvx4J5Pld/+Wp/t7Y6vwIUmZi+936iPk/7i1sXvuPbJ8/gihNHoLHQo5f+cTHbrprhY+qE8LgI8VNIrLHNQUoUgEijo3wIDAQABAoGBAJYh0Rr+fGegfr8lCmvMyN7bPrjOYL4+K6/PG6PsrFQLsYW2ZtTksmtSeitnFunXW4PrANAx0pJI9FZDxQwRFclOeyMkeqkPi6AMIdH66l9QmfiWJ0cpdRIXwpHx0XII9CaN+UpUxzAluhDaCpwL55GJbNqhx0AmnOabCTE45O8xAkEA7uuGWCaFdwT7xTIEHhbxaaEBa5retYZWKGTOWHHpaGeKlWPnan4S86zwUOTPct9zBohDl6ReqUqcUhxAcqzEiQJBAK5zcoaKJICiRAKRq8G8IPz8cGG4+39yDb2Jr4I2RHQdVqezBbK4qCtFb6UyM23xn0NzMZJu1gGJ2T3LuMPROCcCQQC7JUQDcpEizIWGTopJI4GQUuyw5AvFNufwFh5Hy1qgTFKSeEIB+aVQuEs5ojEY8wy/tibz9m2rv+S4sKaO7OO5AkAhCrLNcsrZJmLVTbwHdNeWs0Wh7MSN7g9WMAbzFc4/Y/MuzkStBjasA3nTA0AkedhdkSr5fk7AediQ0M5NIIqPAkBKQUNDL2EPVaz1y/jR6G3VtVuM5chj+Fzpw36SuVX6aus5H/qb9RiXY1WSPury3NU157fXWX2CLFHe5pXtEaZz-----END PRIVATE KEY-----"
            .Replace("-----BEGIN PRIVATE KEY-----", "")
            .Replace("-----END PRIVATE KEY-----", "")
            .Replace("\n", "")
            .Replace("\r", "");

        var keyBytes = Convert.FromBase64String(key);

        var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(keyBytes, out _);
        var exportParameters = rsa.ExportParameters(true);
        return new RsaSecurityKey(exportParameters);
    }

    private RsaSecurityKey GetPublicKey()
    {
        var key = "-----BEGIN PUBLIC KEY-----MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCiz9P/tdCfvRATD8CQVDMRoHuTW3WrLibnewj7DKD93GGOBfVQRrkTOz+FDo+Qs7NxNBGnFByrnFgf2xvx4J5Pld/+Wp/t7Y6vwIUmZi+936iPk/7i1sXvuPbJ8/gihNHoLHQo5f+cTHbrprhY+qE8LgI8VNIrLHNQUoUgEijo3wIDAQAB-----END PUBLIC KEY-----"
            .Replace("-----BEGIN PUBLIC KEY-----", "")
            .Replace("-----END PUBLIC KEY-----", "")
            .Replace("\n", "")
            .Replace("\r", "");
        var keyBytes = Convert.FromBase64String(key);

        var rsa = RSA.Create();
        rsa.ImportSubjectPublicKeyInfo(keyBytes, out _);
        var exportParameters = rsa.ExportParameters(false);
        return new RsaSecurityKey(exportParameters);
    }
}