using AuthForLoreCreator.DbStuff.Models;
using Microsoft.EntityFrameworkCore;
using SharedForLoreCreator.Repositories;

namespace AuthForLoreCreator.DbStuff.Repositories;

public class UserRepository : BaseDbRepository<User, AuthForLoreCreatorDbContext>
{
    public UserRepository(AuthForLoreCreatorDbContext context) : base(context)
    {
    }

    public bool EmailIsExist(string email) => _entyties.Any(x => x.Email == email);

    public bool NameIsExist(string name) => _entyties.Any(x => x.Name == name);

    public void ChangeName(int id, string name)
    {
        var user = _entyties.First(x => x.Id == id);
        if (user is null) return;

        user.Name = name;
        _context.SaveChanges();
    }

    internal bool AnyUserWithName(string name)
    {
        return _entyties.Any(x => x.Name == name);
    }

    public override User? GetById(int id)
    {
        return _entyties.Include( x=> x.Roles).FirstOrDefault(ent => ent.Id == id);
    }
    public User? GetByEmailAndPassword(string email, string password)
    {
        return _entyties.Include(x => x.Roles).FirstOrDefault( x => x.Email == email && x.Password == password);
    }
}
