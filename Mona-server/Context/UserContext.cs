using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mona.Model;

namespace Mona.Context;

public class UserContext(DbContextOptions<UserContext> options) : IdentityDbContext<ApplicationUser>(options)
{
}