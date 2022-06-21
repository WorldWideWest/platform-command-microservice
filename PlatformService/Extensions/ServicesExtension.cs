using Microsoft.EntityFrameworkCore;
using PlatformService.Databse;
using PlatformService.Databse.Repository;
using PlatformService.Models.Interfaces;
using PlatformService.SyncDataService.Http;

namespace PlatformService.Extensions{
    public static class ServicesExtension{


        public static IServiceCollection AddServices(this IServiceCollection services, WebApplicationBuilder builder){
            
            if(builder.Environment.IsProduction())
            {
                services.AddDbContext<ApplicationDbContext>(options => {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformConnectionString"));
                });
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options => {
                    options.UseInMemoryDatabase("InMem");
                });
            }

            services.AddScoped<IPlatformRepository, PlatformRepository>();
            services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
            services.AddSwaggerConfiguration();
            services.AddApiVersioning();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}