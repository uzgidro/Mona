using Microsoft.AspNetCore.Mvc;
using Mona.Enum;

namespace Mona.Controller;

[Controller]
public abstract class MainController : ControllerBase
{
    protected string GetUserId()
    {
        return HttpContext.User.Claims.First(claim => claim.Type.Equals(Claims.Id)).Value;
    }
}