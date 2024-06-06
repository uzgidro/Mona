using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mona.Model;
using Mona.Model.Dto;
using Mona.Service.Interface;

namespace Mona.Service;

public class UserService(UserManager<UserModel> userManager) : IUserService
{
    public async Task<IEnumerable<UserDto>> GetUsersExceptCaller(string? username)
    {
        return await userManager.Users
            .Where(user => !user.Id.Equals(username)).Select(m => new UserDto
            {
                Id = m.Id,
                Name = m.FirstName + " " + m.LastName,
            }).ToListAsync();
    }
}