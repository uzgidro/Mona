using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mona.Context;
using Mona.Model;
using Mona.Model.Dto;
using Mona.Service.Interface;

namespace Mona.Service;

public class UserService(UserManager<UserModel> userManager, ApplicationContext context) : IUserService
{
    public async Task<IEnumerable<UserDto>> GetUsersExceptCaller(string? username)
    {
        return await userManager.Users
            .Where(user => !user.Id.Equals(username)).Select(m => new UserDto
            {
                Id = m.Id,
                Name = m.FirstName + " " + m.LastName,
                ChatId = context.Chats
                    .Where(chat => chat.ChatUsers.Any(cu => cu.ClientId == username) &&
                                   chat.ChatUsers.Any(cu => cu.ClientId == m.Id))
                    .Select(m => m.Id)
                    .FirstOrDefault(),
            }).ToListAsync();
    }

    public async Task<UserModel> GetUserInfo(string id)
    {
        return await userManager.Users.Include(m => m.Groups).FirstAsync(m => string.Equals(m.Id, id));
    }
}