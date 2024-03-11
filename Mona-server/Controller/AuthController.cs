using Microsoft.AspNetCore.Mvc;
using Mona.Context;
using Mona.Model;
using Mona.Service.Interface;

namespace Mona.Controller;

[Route("/auth")]
[ApiController]
public class AuthController(
    ICryptoService cryptoService,
    IJwtService jwtService,
    UserContext userContext) : ControllerBase
{
    [HttpGet("hello")]
    public IActionResult GetSomeItem()
    {
        return Ok();
    }

    [HttpPost("sign-up")]
    public async Task<IResult> SignUp(User user)
    {
        var passwordHash = cryptoService.GetPasswordHash(user.Password);
        user.Password = passwordHash;
        var entityEntry = userContext.Users.Add(user);
        var saveChangesAsync = await userContext.SaveChangesAsync();
        return saveChangesAsync is 0 ? Results.BadRequest() : Results.Created("", entityEntry.Entity);
    }

    [HttpPost("sign-in")]
    public async Task<IResult> SignIn(Login login)
    {
        var user = userContext.Users.FirstOrDefault(user => user.PersonalId == login.PersonalId);
        if (user == null) return Results.Unauthorized();
        var checkPassword = cryptoService.CheckPassword(login.Password, user.Password);
        return checkPassword ? Results.Ok(jwtService.EncodeToken(user)) : Results.Unauthorized();
    }
}