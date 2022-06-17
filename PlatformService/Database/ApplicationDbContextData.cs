using PlatformService.Models.Entities;

namespace PlatformService.Databse{
    public static class ApplicationDbContextData{
        public static void Setup(IApplicationBuilder builder){
            using(var serviceScope = builder.ApplicationServices.CreateScope()){
                SeedData(serviceScope.ServiceProvider.GetService<ApplicationDbContext>());
            }
        }

        private static void SeedData(ApplicationDbContext context){
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