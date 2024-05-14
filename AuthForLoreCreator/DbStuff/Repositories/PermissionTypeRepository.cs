using SharedForLoreCreator.Repositories;
using SharedForLoreCreator.Models;
using AuthForLoreCreator.DbStuff.Models;

namespace AuthForLoreCreator.DbStuff.Repositories;

public class PermissionTypeRepository : BaseDbRepository<PermissionType, AuthForLoreCreatorDbContext>
{
    public PermissionTypeRepository(AuthForLoreCreatorDbContext context) : base(context)
    {
    }
    public PermissionType GetByEnum(PermissionTypes permission)
    {
        return _entyties.First(x => x.Id == permission);
    }
    public IEnumerable<PermissionType> GetManyByEnumList(IEnumerable<PermissionTypes> permissionTypes)
    {
        return _entyties.Where(x => permissionTypes.Contains(x.Id));
    }
}
