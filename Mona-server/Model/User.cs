using Microsoft.EntityFrameworkCore;

namespace Mona.Model;

[Index(nameof(PersonalId), IsUnique = true)]
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PersonalId { get; set; }
    public string Password { get; set; }
}