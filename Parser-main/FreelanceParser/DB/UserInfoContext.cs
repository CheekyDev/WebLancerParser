using FreelanceParser.Model;
using Microsoft.EntityFrameworkCore;

namespace FreelanceParser.DB
{
    public class UserInfoContext  : DbContext
    {
        public DbSet<UserInfo> UserInfos { get; set; }
        
        public UserInfoContext()
        {
            Database.EnsureCreated();
        }
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DB.CONNECTION_STRING);
        }
    }
}