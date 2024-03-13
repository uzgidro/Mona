namespace Mona.Service.Interface;

public interface ICryptoService
{
    string GetPasswordHash(string password);
    bool CheckPassword(string password, string hash);
}