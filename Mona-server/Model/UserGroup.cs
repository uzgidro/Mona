namespace Mona.Model;

public class UserGroup
{
    public string UserId { get; set; }
    public string GroupId { get; set; }
    public UserModel UserModel { get; set; } = null!;
    public GroupModel GroupModel { get; set; } = null!;
}