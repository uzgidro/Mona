using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mona.Model;
using Mona.Service.Interface;

namespace Mona.Service;

public class UserService(UserManager<UserModel> userManager) : IUserService
{
    public async Task<IEnumerable<UserModel>> GetUsersExceptCaller(string? username)
    {
        return await userManager.Users
            .Where(user => !user.Id.Equals(username)).ToListAsync();
    }

    public async Task<UserModel> GetUserInfo(string id)
    {
        return await userManager.Users.Include(m => m.Groups).FirstAsync(m => string.Equals(m.Id, id));
    }
}