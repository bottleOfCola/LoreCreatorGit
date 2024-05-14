using LoreCreator.DbStuff.Models;
using Microsoft.EntityFrameworkCore;
using SharedForLoreCreator.Repositories;

namespace LoreCreator.DbStuff.Repositories;

public class ConnectionRepository : BaseDbRepository<Connection, LoreCreatorDbContext>
{
    public ConnectionRepository(LoreCreatorDbContext context) : base(context)
    {
    }

    public virtual Connection? GetById(int id)
    {
        return _entyties.Include(x => x.Elements).Include(x => x.ConnectionType).First(x => x.Id == id);
    }
}
