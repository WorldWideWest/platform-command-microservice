using Microsoft.EntityFrameworkCore;
using PlatformService.Databse;
using PlatformService.Databse.Repository;
using PlatformService.Models.Interfaces;
using PlatformService.SyncDataService.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

var env = builder.Environment;

 if (env.IsDevelopment())
{
    Console.WriteLine("--> Initializing InMemDB");

    builder.Services.AddDbContext<ApplicationDbContext>(options => {
        options.UseInMemoryDatabase("InMem");
    });
}
else{
    Console.WriteLine("--> Initializing Production DB MS SQL Server");

    builder.Services.AddDbContext<ApplicationDbContext>(options => {
        options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformConnectionString"));
    });
}

builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();

builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddApiVersioning();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.EnableAnnotations();
});

var app = builder.Build();



// Configure the HTTP request pipeline.
 if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.DefaultModelsExpandDepth(-1);
    });
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

ApplicationDbContextData.Setup(app);

app.Run();
