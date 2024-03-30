using Microsoft.AspNetCore.Identity;

namespace Mona.Model;

public class UserModel : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}