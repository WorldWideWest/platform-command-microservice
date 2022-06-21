using PlatformService.Databse;
using PlatformService.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddServices(builder);

// Add Services Collection from Extensions

var app = builder.Build();
 if (app.Environment.IsDevelopment())
    app.UseSwaggerConfiguration();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

ApplicationDbContextData.Setup(app, app.Environment.IsProduction());

app.Run();
