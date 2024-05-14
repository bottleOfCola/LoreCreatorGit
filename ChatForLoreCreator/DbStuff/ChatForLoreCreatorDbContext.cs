using ChatForLoreCreator.DbStuff.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatForLoreCreator.DbStuff;

public class ChatForLoreCreatorDbContext : DbContext
{
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }


    public ChatForLoreCreatorDbContext(DbContextOptions<ChatForLoreCreatorDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Chat>().HasMany(x=> x.Messages).WithOne(x => x.Chat);
    }
}
