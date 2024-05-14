using AuthForLoreCreator.DbStuff.Models;
using Microsoft.EntityFrameworkCore;
using SharedForLoreCreator.Models;

namespace AuthForLoreCreator.DbStuff;

public class AuthForLoreCreatorDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<PermissionType> Permissions { get; set; }

    public AuthForLoreCreatorDbContext(DbContextOptions<AuthForLoreCreatorDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().HasMany(x => x.Roles).WithMany(x => x.Users);
        builder.Entity<Role>().HasMany(x => x.Permissions).WithMany(x => x.Roles);
    }
}
