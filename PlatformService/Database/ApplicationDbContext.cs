using Microsoft.EntityFrameworkCore;
using PlatformService.Models.Entities;

namespace PlatformService.Databse{

    public class ApplicationDbContext : DbContext{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Platform> Platforms { get; set; }
    }

}

