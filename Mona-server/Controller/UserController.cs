using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mona.Service.Interface;

namespace Mona.Controller;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : MainController
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserInfo(string id)
    {
        return Ok(await userService.GetUserInfo(id));
    }

    // [HttpGet("list")]
    // public async Task<IResult> GetUserList()
    // {
    //     return Results.Json(userManager.Users);
    // }
}