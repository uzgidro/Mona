using Mona.Service.Interface;

namespace Mona.Service;

public class CryptoService : ICryptoService
{
    public string GetPasswordHash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool CheckPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}