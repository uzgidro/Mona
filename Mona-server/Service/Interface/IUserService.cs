using Mona.Model;
using Mona.Model.Dto;

namespace Mona.Service.Interface;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetUsersExceptCaller(string? username);
    Task<UserModel> GetUserInfo(string id);
}