using Landsacper.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Landsacper.DAL
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
   

        public AppDbContext(DbContextOptions options) : base(options) 
        {
            
        }

        public DbSet<Service> Services { get; set; }
    }
}
