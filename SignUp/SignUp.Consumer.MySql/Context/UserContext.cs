using Microsoft.EntityFrameworkCore;
using SignUp.Entities;

namespace SignUp.Consumers.Context
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
    }

}
