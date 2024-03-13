using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mona.Model;
using Mona.Service.Interface;
using RegisterRequest = Mona.Model.RegisterRequest;

namespace Mona.Controller;

[Route("/auth")]
[ApiController]
public class AuthController(
    ICryptoService cryptoService,
    IJwtService jwtService,
    UserManager<ApplicationUser> userManager) : ControllerBase
{
    [HttpPost("sign-up")]
    public async Task<IResult> SignUp(RegisterRequest registerRequest)
    {
        var user = new ApplicationUser
        {
            UserName = registerRequest.UserName,
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            PasswordHash = cryptoService.GetPasswordHash(registerRequest.Password)
        };
        var identityResult = await userManager.CreateAsync(user);
        return identityResult.Succeeded ? Results.Created() : Results.BadRequest();
    }

    [HttpPost("sign-in")]
    public async Task<IResult> SignIn(CustomLoginRequest loginRequest)
    {
        var user = await userManager.FindByNameAsync(loginRequest.UserName);
        if (user == null) return Results.Unauthorized();
        var checkPassword = cryptoService.CheckPassword(loginRequest.Password, user.PasswordHash);
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