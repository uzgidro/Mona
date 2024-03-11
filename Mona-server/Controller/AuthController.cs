using Microsoft.AspNetCore.Mvc;
using Mona.Context;
using Mona.Model;
using Mona.Service.Interface;

namespace Mona.Controller;

[Route("/auth")]
[ApiController]
public class AuthController(ICryptoService cryptoService, UserContext userContext) : ControllerBase
{
    [HttpGet("hello")]
    public IActionResult GetSomeItem()
    {
        return Ok();
    }

    [HttpPost("sign-up")]
    public async Task<IResult> PostSomething(User user)
    {
        var passwordHash = cryptoService.GetPasswordHash(user.Password);
        user.Password = passwordHash;
        var entityEntry = userContext.Users.Add(user);
        var saveChangesAsync = await userContext.SaveChangesAsync();
        return saveChangesAsync is 0 ? Results.BadRequest() : Results.Created("", entityEntry.Entity);
    }
}