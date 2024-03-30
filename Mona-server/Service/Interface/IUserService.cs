using Mona.Model;

namespace Mona.Service.Interface;

public interface IUserService
{
    Task<IEnumerable<UserModel>> GetUsersExceptCaller(string? username);
}