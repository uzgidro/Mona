using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Mona.Enum;
using Mona.Model;
using Mona.Service.Interface;

namespace Mona.Service;

public class JwtService(IConfiguration configuration) : IJwtService
{
    public TokenPair EncodeTokenPair(ApplicationUser applicationUser)
    {
        var credentials = GetCredentials();
        var access = new JwtSecurityToken(
            issuer: "Server",
            audience: "Client",
            claims: new List<Claim>
            {
                new(Claims.Id, applicationUser.Id),
                new(Claims.Username, applicationUser.UserName),
                new(Claims.FirstName, applicationUser.LastName),
                new(Claims.LastName, applicationUser.LastName),
            },
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        var refresh = new JwtSecurityToken(
            issuer: "Server",
            audience: "Client",
            claims: new List<Claim>
            {
                new(Claims.Id, applicationUser.Id),
                new(Claims.Username, applicationUser.UserName),
            },
            expires: DateTime.UtcNow.AddHours(24),
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
            var idFromAccess = DecodeToken(tokens.AccessToken, false).Claims
                .FirstOrDefault(claim => claim.Type.Equals(Claims.Id)).Value;
            var idFromRefresh = DecodeToken(tokens.RefreshToken).Claims
                .FirstOrDefault(claim => claim.Type.Equals(Claims.Id)).Value;
            if (idFromAccess == idFromRefresh)
            {
                return idFromAccess;
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
        var key = configuration["PrivateKey"]
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
        var key = configuration["PublicKey"]
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