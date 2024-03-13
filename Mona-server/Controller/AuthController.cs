using Microsoft.AspNetCore.Authorization;
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
    public async Task<IResult> SignUp(RegisterRequest registerRequest)
    {
        var passwordHash = cryptoService.GetPasswordHash(registerRequest.Password);
        registerRequest.Password = passwordHash;
        var user = new ApplicationUser
        {
            PersonalId = registerRequest.PersonalId,
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            PasswordHash = registerRequest.Password
        };
        var entityEntry = userContext.Users.Add(user);
        var saveChangesAsync = await userContext.SaveChangesAsync();
        return saveChangesAsync is 0 ? Results.BadRequest() : Results.Created("", entityEntry.Entity);
    }

    [HttpPost("sign-in")]
    public IResult SignIn(CustomLoginRequest customLoginRequest)
    {
        var user = userContext.Users.FirstOrDefault(user => user.PersonalId == customLoginRequest.PersonalId);
        if (user == null) return Results.Unauthorized();
        var checkPassword = cryptoService.CheckPassword(customLoginRequest.Password, user.PasswordHash);
        if (!checkPassword) return Results.Unauthorized();
        var response = new AuthResponse(jwtService.EncodeToken(user), jwtService.EncodeRefreshToken(user));
        return Results.Json(response);
    }

    [HttpGet("secure")]
    [Authorize]
    public IResult Secure()
    {
        return Results.Ok();
    }
}