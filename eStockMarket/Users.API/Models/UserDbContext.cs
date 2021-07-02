using Microsoft.EntityFrameworkCore;

namespace Users.API.Models
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<UserDetails> Users { get; set; }
    }
}
