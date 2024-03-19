using Mona.Model;

namespace Mona.Service.Interface;

public interface IUserService
{
    Task<IEnumerable<ApplicationUser>> GetUsersExceptCaller(string username);
}