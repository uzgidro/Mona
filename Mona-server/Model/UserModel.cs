using Microsoft.AspNetCore.Identity;

namespace Mona.Model;

public class UserModel : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public IEnumerable<GroupModel> Groups { get; init; } = new List<GroupModel>();
    public IEnumerable<UserGroup> UserGroups { get; init; } = new List<UserGroup>();
}