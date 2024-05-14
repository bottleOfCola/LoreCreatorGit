using LoreCreator.DbStuff.Models;
using SharedForLoreCreator.Repositories;

namespace LoreCreator.DbStuff.Repositories;

public class ConnectionTypeRepository : BaseDbRepository<ConnectionType, LoreCreatorDbContext>
{
    public ConnectionTypeRepository(LoreCreatorDbContext context) : base(context)
    {
    }
}
