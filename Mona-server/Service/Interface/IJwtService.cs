using Mona.Model;

namespace Mona.Service.Interface;

public interface IJwtService
{
    string EncodeToken(User user);
    void DecodeToken(string token);
    bool ValidToken(string token);
}