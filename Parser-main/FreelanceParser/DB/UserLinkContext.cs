using FreelanceParser.Model;
using Microsoft.EntityFrameworkCore;

namespace FreelanceParser.DB
{
    public class UserLinkContext  : DbContext
    {
        public DbSet<UserLink> UserLinks { get; set; }
         
        public UserLinkContext()
        {
            Database.EnsureCreated();
        }
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DB.CONNECTION_STRING);
        }
    }
}