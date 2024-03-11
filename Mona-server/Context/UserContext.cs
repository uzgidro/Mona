using Microsoft.EntityFrameworkCore;
using Mona.Model;

namespace Mona.Context;

public class UserContext(DbContextOptions<UserContext> options): DbContext(options)
{
    public DbSet<User> Users { get; set; }
}