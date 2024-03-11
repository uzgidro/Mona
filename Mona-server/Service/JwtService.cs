using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Mona.Model;
using Mona.Service.Interface;

namespace Mona.Service;

public class JwtService(IConfiguration configuration) : IJwtService
{
    public string EncodeToken(User user)
    {
        try
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
            var securityKey = new RsaSecurityKey(exportParameters);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);
            var token = new JwtSecurityToken(
                issuer: "Server",
                audience: "Client",
                claims: new List<Claim> { new Claim(ClaimTypes.Name, user.LastName + " " + user.FirstName) },
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

    public void DecodeToken(string token)
    {
    }

    public bool ValidToken(string token)
    {
        try
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
            var securityKey = new RsaSecurityKey(exportParameters);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "Server",
                IssuerSigningKey = securityKey
            };
            var handler = new JwtSecurityTokenHandler();
            handler.ValidateToken(token, validationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }
}