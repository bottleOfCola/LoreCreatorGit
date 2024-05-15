using LoreCreator.LocalizationFiles;
using SharedForLoreCreator.Models;
using System.Security.Claims;

namespace LoreCreator.Services;

public static class Supporter
{
    public static string GetNameOfPermission(PermissionTypes permission)
        => permission switch
        {
            PermissionTypes.Unknown => LCLPermissionTypes.Unknown,
            PermissionTypes.AddMessage => LCLPermissionTypes.AddMessage,
            PermissionTypes.RemoveMessage => LCLPermissionTypes.RemoveMessage,
            PermissionTypes.AddElement => LCLPermissionTypes.AddElement,
            PermissionTypes.RemoveElement => LCLPermissionTypes.RemoveElement,
            PermissionTypes.AddConnection => LCLPermissionTypes.AddConnection,
            PermissionTypes.RemoveConnection => LCLPermissionTypes.RemoveConnection,
            PermissionTypes.AddConnectionType => LCLPermissionTypes.AddConnectionType,
            PermissionTypes.RemoveConnectionType => LCLPermissionTypes.RemoveConnectionType,
            PermissionTypes.AddTag => LCLPermissionTypes.AddTag,
            PermissionTypes.RemoveTag => LCLPermissionTypes.RemoveTag,
            PermissionTypes.RoleGiving => LCLPermissionTypes.RoleGiving,
            PermissionTypes.RoleBacking => LCLPermissionTypes.RoleBacking,
            PermissionTypes.AddRole => LCLPermissionTypes.AddRole,
            PermissionTypes.RemoveRole => LCLPermissionTypes.RemoveRole,
            _ => LCLPermissionTypes.Error,
        };
    public static List<PermissionTypes> GetPermissionsFromString(IEnumerable<Claim> claims)
        => claims.FirstOrDefault(x => x.Type == "permissions")?.Value.Split(' ').ToList().ConvertAll(x => (PermissionTypes)int.Parse(x)) ?? new List<PermissionTypes>();
}
