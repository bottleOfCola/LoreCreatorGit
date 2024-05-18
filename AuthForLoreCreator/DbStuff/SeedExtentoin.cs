using AuthForLoreCreator.DbStuff.Models;
using AuthForLoreCreator.DbStuff.Repositories;
using SharedForLoreCreator.Models;

namespace AuthForLoreCreator.DbStuff;

public static class SeedExtentoin
{
    public const string ADMIN_ROLE = "Admin";
    public const string MODERATOR_ROLE = "Moderator";
    public const string USER_ROLE = "User";

    public static void Seed(WebApplication app)
    {
        using (var serviceScope = app.Services.CreateScope())
        {
            SeedPermissions(serviceScope.ServiceProvider);
            SeedRoles(serviceScope.ServiceProvider);
            SeedUsers(serviceScope.ServiceProvider);
            SeedModels(serviceScope.ServiceProvider);
        }
    }

    private static void SeedModels(IServiceProvider serviceProvider)
    {
        var aRepository = serviceProvider.GetService<ModelARepository>();
        var bRepository = serviceProvider.GetService<ModelBRepository>();

        int aId = 0;
        int bId = 0;
        if (!aRepository.Any())
        {
            aId = aRepository.Add(new()
            {
                Name = "Lalka"
            });
        }
        if (!bRepository.Any())
        {
            bId = bRepository.Add(new()
            {
                Name = "Balka"
            });
        }
        ModelAForOneToOne? a = aRepository.GetById(aId);
        ModelBForOneToOne? b = bRepository.GetById(aId);
        if(a is not null && b is not null)
        {
            aRepository.AddBModel(aId, b);
            bRepository.AddAModel(bId, a);
        }
    }

    private static void SeedUsers(IServiceProvider serviceProvider)
    {
        var userRepository = serviceProvider.GetService<UserRepository>();
        if (!userRepository.AnyUserWithName(ADMIN_ROLE))
        {
            userRepository.Add(new()
            {
                Name = ADMIN_ROLE,
                Password = "123321",
                Email = "bruh@kek.lol",
                Roles = new() { GetRole(serviceProvider, ADMIN_ROLE)!}
            });
        }
        if(!userRepository.AnyUserWithName(USER_ROLE))
        {
            userRepository.Add(new()
            {
                Name = USER_ROLE,
                Password = "321123",
                Email = "gangstaLaugh@rzhaka.lol",
                Roles = new() { GetRole(serviceProvider, USER_ROLE)! }
            });
        }
    }

    private static void SeedRoles(IServiceProvider di)
    {
        var roleRepository = di.GetService<RoleRepository>();
        var permissionRepository = di.GetService<PermissionTypeRepository>();

        if (!roleRepository.Any())
        {
            roleRepository.Add(new()
            {
                Name = ADMIN_ROLE,
                Permissions = new()
                {
                    permissionRepository.GetByEnum(PermissionTypes.AddMessage),
                    permissionRepository.GetByEnum(PermissionTypes.AddRole),
                    permissionRepository.GetByEnum(PermissionTypes.AddElement),
                    permissionRepository.GetByEnum(PermissionTypes.AddConnection),
                    permissionRepository.GetByEnum(PermissionTypes.AddConnectionType),
                    permissionRepository.GetByEnum(PermissionTypes.AddTag),
                    permissionRepository.GetByEnum(PermissionTypes.RemoveMessage),
                    permissionRepository.GetByEnum(PermissionTypes.RemoveRole),
                    permissionRepository.GetByEnum(PermissionTypes.RemoveElement),
                    permissionRepository.GetByEnum(PermissionTypes.RemoveConnection),
                    permissionRepository.GetByEnum(PermissionTypes.RemoveConnectionType),
                    permissionRepository.GetByEnum(PermissionTypes.RemoveTag),
                    permissionRepository.GetByEnum(PermissionTypes.RoleGiving),
                    permissionRepository.GetByEnum(PermissionTypes.RoleBacking)
                }
            });
            roleRepository.Add(new()
            {
                Name = USER_ROLE,
                Permissions = new()
                {
                    permissionRepository.GetByEnum(PermissionTypes.AddMessage),
                }
            });
            roleRepository.Add(new()
            {
                Name = "Trusted",
                Permissions = new()
                {
                    permissionRepository.GetByEnum(PermissionTypes.AddMessage),
                    permissionRepository.GetByEnum(PermissionTypes.RemoveMessage),
                    permissionRepository.GetByEnum(PermissionTypes.AddElement),
                    permissionRepository.GetByEnum(PermissionTypes.AddConnection),
                    permissionRepository.GetByEnum(PermissionTypes.AddConnectionType),
                    permissionRepository.GetByEnum(PermissionTypes.AddTag),
                    permissionRepository.GetByEnum(PermissionTypes.RemoveElement),
                    permissionRepository.GetByEnum(PermissionTypes.RemoveConnection),
                    permissionRepository.GetByEnum(PermissionTypes.RemoveConnectionType),
                    permissionRepository.GetByEnum(PermissionTypes.RemoveTag),
                }
            });
        }
    }


    private static void SeedPermissions(IServiceProvider serviceProvider)
    {
        var permissionRepository = serviceProvider.GetService<PermissionTypeRepository>();
        if (!permissionRepository.Any())
        {
            foreach (var perm in Enum.GetValues<PermissionTypes>())
            {
                permissionRepository.Add(new()
                {
                    Id = perm
                });
            }
        }
    }

    private static Role? GetRole(IServiceProvider di, string roleName)
    {
        var roleRepository = di.GetService<RoleRepository>();
        var role = roleRepository.GetByName(roleName);
        return role;
    }
}
