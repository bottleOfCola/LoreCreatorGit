using LoreCreator.DbStuff.Models;
using Microsoft.EntityFrameworkCore;

namespace LoreCreator.DbStuff;

public class LoreCreatorDbContext : DbContext
{
    public DbSet<Element> Elements { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<ConnectionType> ConnectionTypes { get; set; }
    public DbSet<Connection> Connections { get; set; }
    public LoreCreatorDbContext(DbContextOptions<LoreCreatorDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Element>().HasMany(e => e.Tags).WithMany(t => t.Elements);
        builder.Entity<Element>().HasMany(e => e.Connections).WithMany(c => c.Elements);

        builder.Entity<Connection>().HasOne(c => c.ConnectionType).WithMany().OnDelete(DeleteBehavior.NoAction);
    }
}
