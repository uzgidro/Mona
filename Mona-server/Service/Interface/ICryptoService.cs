namespace Mona.Service.Interface;

public interface ICryptoService
{
    string GetPasswordHash(string password);
}