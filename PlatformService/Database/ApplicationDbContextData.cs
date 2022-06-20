using Microsoft.EntityFrameworkCore;
using PlatformService.Models.Entities;

namespace PlatformService.Databse{
    public static class ApplicationDbContextData{
        public static void Setup(IApplicationBuilder builder, bool isProd){
            using(var serviceScope = builder.ApplicationServices.CreateScope()){
                SeedData(serviceScope.ServiceProvider.GetService<ApplicationDbContext>(), isProd);
            }
        }

        private static void SeedData(ApplicationDbContext context, bool isProd){
            if(isProd)
                context.Database.Migrate();
            else{
                if(!context.Platforms.Any()){
                    Console.WriteLine("--> Seeding data...");
                    context.Platforms.AddRange(
                        new Platform() { Name = ".NET Core", Publisher = "Microsoft", Cost = "Free" },
                        new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                        new Platform() { Name = "Kubernetes", Publisher = "Google", Cost = "Free" }
                    );
                    context.SaveChanges();
                }
                else
                    Console.WriteLine("--> We have data");
            }

            
        }
    }

}