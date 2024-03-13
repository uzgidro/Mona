using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Mona.Model;
using Mona.Service.Interface;

namespace Mona.Service;

public class JwtService(IConfiguration configuration) : IJwtService
{
    public string EncodeToken(ApplicationUser applicationUser)
    {
        try
        {
            var securityKey = GetPrivateKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);
            var token = new JwtSecurityToken(
                issuer: "Server",
                audience: "Client",
                claims: new List<Claim>
                {
                    new("id", applicationUser.Id),
                    new("personalId", applicationUser.PersonalId),
                    new("name", applicationUser.LastName),
                    new("surname", applicationUser.LastName),
                },
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch
        {
            return "";
        }
    }

    public string EncodeRefreshToken(ApplicationUser applicationUser)
    {
        try
        {
            var securityKey = GetPrivateKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);
            var token = new JwtSecurityToken(
                issuer: "Server",
                audience: "Client",
                claims: new List<Claim>
                {
                    new("id", applicationUser.Id),
                    new("personalId", applicationUser.PersonalId),
                },
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch
        {
            return "";
        }
    }

    public TokenValidationParameters getValidationParameters()
    {
        var securityKey = GetPublicKey();
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "Server",
            ValidAudiences = new List<string> { "Client", "Web", "Mobile" },
            IssuerSigningKey = securityKey,
        };
    }

    public ClaimsPrincipal? DecodeToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityKey = GetPublicKey();
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "Server",
            ValidAudiences = new List<string> { "Client", "Web", "Mobile" },
            IssuerSigningKey = securityKey,
        };
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.RsaSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");
        return principal;
    }

    public bool ValidToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = GetPublicKey();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "Server",
                ValidAudiences = new List<string> { "Client", "Web", "Mobile" },
                IssuerSigningKey = securityKey,
            };
            tokenHandler.ValidateToken(token, validationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
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