using ChatForLoreCreator.DbStuff;
using ChatForLoreCreator.DbStuff.Repositories;
using ChatForLoreCreator.Moddlewares;
using ChatForLoreCreator.Services;
using ChatForLoreCreator.SignalRChatHubs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ChatForLoreCreator");

builder.Services.AddDbContext<ChatForLoreCreatorDbContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddControllers();

//Repositories
builder.Services.AddScoped<ChatRepository>();
builder.Services.AddScoped<MessageRepository>();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddSingleton<AuthSericeWorker>();

builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.SetIsOriginAllowed(url => true);
        policy.AllowCredentials();
    });
});

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

var app = builder.Build();

app.UseCors();

app.UseMiddleware<AuthForServicesMiddleware>();

app.UseStaticFiles();

app.MapHub<ChatHub>("/chating");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Chat}/{action=Index}");

app.Run();
