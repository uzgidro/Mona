using Mona.Service.Interface;

namespace Mona.Service;

public class CryptoService : ICryptoService
{
    public string GetPasswordHash(string password)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        BCrypt.Net.BCrypt.Verify(password, hash);
        return hash;
    }

    public bool CheckPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}