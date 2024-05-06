using System.ComponentModel.DataAnnotations.Schema;

namespace Mona.Model;

[Table("Groups")]
public class GroupModel
{
    public string Id { get; init; } = string.Concat("g-", Guid.NewGuid().ToString());
    public string Name { get; set; }
    public string? Description { get; set; }
    public IEnumerable<UserModel> Users { get; init; } = new List<UserModel>();
}