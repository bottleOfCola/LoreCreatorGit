using LoreCreator.Controllers;
using LoreCreator.DbStuff;
using LoreCreator.DbStuff.Repositories;
using LoreCreator.Middlewares;
using LoreCreator.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(AuthController.AUTH_KEY)
    .AddCookie(AuthController.AUTH_KEY, option =>
    {
        option.LoginPath = "/Auth/Login";
    });

var connectionString = builder.Configuration.GetConnectionString("LoreCreator");

builder.Services.AddDbContext<LoreCreatorDbContext>(x => x.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ElementRepository>();
builder.Services.AddScoped<ConnectionTypeRepository>();
builder.Services.AddScoped<ConnectionRepository>();
builder.Services.AddScoped<TagRepository>();
builder.Services.AddScoped<UserRepository>();

builder.Services.AddScoped<ChatServiceWorker>();
builder.Services.AddScoped<AuthServiceWorker>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();

app.UseMiddleware<LocalizationMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=LoreCreator}/{action=Index}");

app.Run();
