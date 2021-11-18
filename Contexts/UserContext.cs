using Microsoft.EntityFrameworkCore;
using RESTfulXLS.Models;

namespace RESTfulXLS.Contexts
{
    public class UserContext: DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }


        public DbSet<User> Users {  get; set; }
    }
}
