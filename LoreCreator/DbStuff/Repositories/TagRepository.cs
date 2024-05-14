using LoreCreator.DbStuff.Models;
using Microsoft.EntityFrameworkCore;
using SharedForLoreCreator.Repositories;

namespace LoreCreator.DbStuff.Repositories;

public class TagRepository : BaseDbRepository<Tag, LoreCreatorDbContext>
{
    public TagRepository(LoreCreatorDbContext context) : base(context)
    {
    }

    public virtual Tag? GetById(int id)
    {
        return _entyties.Include(x => x.Elements).First(x => x.Id == id);
    }
}
