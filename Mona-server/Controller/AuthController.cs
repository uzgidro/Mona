using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Mona.Model;
using Mona.Model.Dto;
using Mona.Service.Interface;

namespace Mona.Controller;

[Route("[controller]")]
[ApiController]
public class AuthController(
    ICryptoService cryptoService,
    IJwtService jwtService,
    UserManager<UserModel> userManager) : MainController
{
    [HttpPost("sign-up")]
    public async Task<IResult> SignUp(CustomRegisterRequest customRegisterRequest)
    {
        var user = new UserModel
        {
            UserName = customRegisterRequest.Username,
            FirstName = customRegisterRequest.FirstName,
            LastName = customRegisterRequest.LastName,
            PasswordHash = cryptoService.GetPasswordHash(customRegisterRequest.Password)
        };
        var identityResult = await userManager.CreateAsync(user);
        if (identityResult.Errors.FirstOrDefault(error => error.Code.Equals("DuplicateUserName")) != null)
        {
            return Results.Conflict();
        }

        return identityResult.Succeeded ? Results.Created() : Results.BadRequest();
    }

    [HttpPost("sign-in")]
    public async Task<IResult> SignIn(CustomLoginRequest loginRequest)
    {
        var user = await userManager.FindByNameAsync(loginRequest.Username);
        if (user == null) return Results.BadRequest();
        var checkPassword = cryptoService.CheckPassword(loginRequest.Password, user.PasswordHash);
        if (!checkPassword) return Results.BadRequest();
        var tokens = jwtService.EncodeTokenPair(user);
        return Results.Json(tokens);
    }

    [HttpPost("refresh")]
    public async Task<IResult> RefreshToken(TokenPair tokenPair)
    {
        var username = jwtService.RefreshTokens(tokenPair);
        if (username.IsNullOrEmpty()) return Results.Unauthorized();
        var user = await userManager.FindByNameAsync(username);
        if (user == null) return Results.BadRequest();
        var tokens = jwtService.EncodeTokenPair(user);
        return Results.Json(tokens);
    }
}