using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mona.Model;

[Index(nameof(PersonalId), IsUnique = true)]
public class ApplicationUser : IdentityUser
{
    public string PersonalId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}