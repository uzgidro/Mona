using Mona.Model;

namespace Mona.Service.Interface;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetUsersExceptCaller(string username);
}