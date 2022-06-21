using Microsoft.OpenApi.Models;

namespace PlatformService.Extensions{
    public static class SwaggerConfiguration{
        public static void AddSwaggerConfiguration(this IServiceCollection services){
            
            services.AddSwaggerGen(options => {
                
                options.SwaggerDoc("v1", new OpenApiInfo{
                    Title = "Platform Service Api",
                    Version = "v1",
                    Description = "Platform Service"
                });

                options.EnableAnnotations();
            });
        }

        public static void UseSwaggerConfiguration(this IApplicationBuilder app){
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.DefaultModelsExpandDepth(-1);
            });
        }
    }
}