using Microsoft.AspNetCore.Identity;

namespace Mona.Model;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<MessageItem> SentMessages { get; set; }
    public ICollection<MessageItem> ReceivedMessages { get; set; }
}