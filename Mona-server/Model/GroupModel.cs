using System.ComponentModel.DataAnnotations.Schema;

namespace Mona.Model;

[Table("Groups")]
public class GroupModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<UserModel> Users { get; } = [];
}