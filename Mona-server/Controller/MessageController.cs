using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mona.Controller;

[Authorize]
[ApiController]
[Route("/message")]
public class MessageController
{
    
}