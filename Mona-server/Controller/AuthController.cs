using Microsoft.AspNetCore.Mvc;
using Mona.Model;

namespace Mona.Controller;

[Route("/auth")]
[ApiController]
public class AuthController: ControllerBase
{
    [HttpGet("hello")]
    public IActionResult GetSomeItem()
    {
        return Ok();
    }
    
    [HttpPost("sign-up")]
    public async Task<IResult> PostSomething(User user)
    {
        return Results.Created("", user);
    }
    
}