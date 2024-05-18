using LoreCreator.DbStuff.Models;
using Microsoft.EntityFrameworkCore;
using SharedForLoreCreator.Repositories;

namespace LoreCreator.DbStuff.Repositories;

public class ElementRepository : BaseDbRepository<Element, LoreCreatorDbContext>
{
    public ElementRepository(LoreCreatorDbContext context) : base(context)
    {
    }
    public override int Add(Element dbModel)
    {
        List<Tag> tags = dbModel.Tags;
        _entyties.Add(dbModel);
        _context.SaveChanges();
        if (tags is not null && tags.Count > 0)
        {
            dbModel.Tags.AddRange(tags);
            _context.SaveChanges();
        }
        return dbModel.Id;
    }

    public virtual Element? GetById(int id)
    {
        return _entyties.Include(x => x.Tags).Include(x => x.Connections).First(x => x.Id == id);
    }

    internal void UpdateAvatar(int id, string avatarUrl)
    {
        GetById(id).Image = avatarUrl;
        _context.SaveChanges();
    }
    internal void UpdateName(int id, string name)
    {
        GetById(id).Name = name;
        _context.SaveChanges();
    }
    internal void UpdateDescription(int id, string description)
    {
        GetById(id).Description = description;
        _context.SaveChanges();
    }

    internal void AddTag(int id, Tag tag)
    {
        GetById(id).Tags.Add(tag);
        _context.SaveChanges();
    }
    
    internal void RemoveTag(int elementId, Tag tag)
    {
        GetById(elementId).Tags.Remove(tag);
        _context.SaveChanges();
    }
}
