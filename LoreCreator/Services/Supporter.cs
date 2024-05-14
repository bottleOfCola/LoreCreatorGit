using LoreCreator.LocalizationFiles;
using SharedForLoreCreator.Models;
using System.Security.Claims;

namespace LoreCreator.Services;

public static class Supporter
{
    public static string GetNameOfPermission(PermissionTypes permission)
        => permission switch
        {
            PermissionTypes.Unknown => "Ошибка",
            PermissionTypes.AddMessage => "Отправка сообщений",
            PermissionTypes.RemoveMessage => "Удаление сообщений",
            PermissionTypes.AddElement => "Добавление элемента",
            PermissionTypes.RemoveElement => "Удаление элемента",
            PermissionTypes.AddTag => "Добавление тэга",
            PermissionTypes.RemoveTag => "Удаление тэга",
            PermissionTypes.AddConnectionType => "Добавление типа связи",
            PermissionTypes.RemoveConnectionType => "Удаление типа связи",
            PermissionTypes.AddConnection => "Добавление связи",
            PermissionTypes.RemoveConnection => "Удаление связи"
        };
    public static List<PermissionTypes> GetPermissionsFromString(IEnumerable<Claim> claims)
        => claims.FirstOrDefault(x => x.Type == "permissions")?.Value.Split(' ').ToList().ConvertAll(x => (PermissionTypes)int.Parse(x)) ?? new List<PermissionTypes>();
}
