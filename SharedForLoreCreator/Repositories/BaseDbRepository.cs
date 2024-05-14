using SharedForLoreCreator.Models;
using Microsoft.EntityFrameworkCore;

namespace SharedForLoreCreator.Repositories;

public abstract class BaseDbRepository<DbModel, DbCon> : IRepository<DbModel>
    where DbModel : BaseModel
    where DbCon : DbContext
{
    protected readonly DbCon _context;
    protected readonly DbSet<DbModel> _entyties;

    public BaseDbRepository(DbCon context)
    {
        _context = context;
        _entyties = _context.Set<DbModel>();
    }

    public virtual bool isExist(int id) => _entyties.Any(x => x.Id == id);

    public virtual DbModel? GetById(int id)
    {
        return _entyties.FirstOrDefault(ent => ent.Id == id);
    }

    public virtual int Add(DbModel dbModel)
    {
        _entyties.Add(dbModel);
        _context.SaveChanges();
        return dbModel.Id;
    }

    public virtual void Delete(int id)
    {
        var weapon = _entyties.First(x => x.Id == id);
        _entyties.Remove(weapon);
        _context.SaveChanges();
    }

    public virtual IEnumerable<DbModel> GetAll()
    {
        return _entyties
            .ToList();
    }

    public virtual bool Any() => _entyties.Any();
}
