using Microsoft.AspNetCore.Mvc;
using Mona.Model;
using Mona.Service.Interface;

namespace Mona.Controller;

[Route("/auth")]
[ApiController]
public class AuthController(ICryptoService cryptoService): ControllerBase
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
        
        return Results.Created("", passwordHash);
    }
    
}