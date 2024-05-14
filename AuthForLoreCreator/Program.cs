using AuthForLoreCreator.DbStuff;
using AuthForLoreCreator.DbStuff.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("AuthForLoreCreator");

builder.Services.AddDbContext<AuthForLoreCreatorDbContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddControllers();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<PermissionTypeRepository>();

var app = builder.Build();

SeedExtentoin.Seed(app);

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Index}");

app.Run();
