using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mona.Model;

namespace Mona.Controller;

[Authorize]
[ApiController]
[Route("/users")]
public class UserController(UserManager<ApplicationUser> userManager) : ControllerBase
{
    [HttpGet("list")]
    public async Task<IResult> GetUserList()
    {
        return Results.Json(userManager.Users);
    }
}