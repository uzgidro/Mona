using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mona.Model;
using Mona.Service.Interface;

namespace Mona.Service;

public class UserService(UserManager<ApplicationUser> userManager) : IUserService
{
    public async Task<IEnumerable<ApplicationUser>> GetUsersExceptCaller(string? username)
    {
        return await userManager.Users
            .Where(user => !user.Id.Equals(username)).ToListAsync();
    }
}