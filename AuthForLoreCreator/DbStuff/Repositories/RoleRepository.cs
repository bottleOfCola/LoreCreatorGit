using AuthForLoreCreator.DbStuff.Models;
using Microsoft.EntityFrameworkCore;
using SharedForLoreCreator.Models;
using SharedForLoreCreator.Repositories;

namespace AuthForLoreCreator.DbStuff.Repositories;

public class RoleRepository : BaseDbRepository<Role, AuthForLoreCreatorDbContext>
{
    public RoleRepository(AuthForLoreCreatorDbContext context) : base(context)
    {
    }
    public override Role? GetById(int id)
    {
        return _entyties.Include(x => x.Permissions).FirstOrDefault(x => x.Id == id);
    }
    public Role? GetByName(string name)
    {
        return _entyties.Include(x=> x.Permissions).FirstOrDefault(x => x.Name == name);
    }
    public bool isExistByName(string name)
    {
        return _entyties.Any(x => x.Name == name);
    }
    public bool isHavePermission(int id, PermissionTypes permission)
    {
        var a = _entyties.Include(x => x.Permissions).FirstOrDefault(x => x.Id == id);
        var b = a.Permissions.Any(x => x.Id == permission);
        return b;
    }
}
