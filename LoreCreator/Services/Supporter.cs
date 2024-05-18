using LoreCreator.LocalizationFiles;
using SharedForLoreCreator.Models;
using System.Security.Claims;

namespace LoreCreator.Services;

public static class Supporter
{
    private static List<string> _languages = new() { "en" };
    public static void Start()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        var directoryes = directory?.GetDirectories();
        var localizationDirectory = directoryes?.FirstOrDefault(x => x.Name == "LocalizationFiles");
        var localizationFiles = localizationDirectory?.GetFiles();
        var preLanguages = localizationFiles?.Select(x => x.Name.Split('.')).Where(x => x.Length == 3 && x[2] == "resx").ToList();
        var languages = preLanguages?.Select( x => x[1]).Distinct().ToList();
        List<string> result = new() { "en" };
        if (languages is not null && languages.Any())
        {
            result.AddRange(languages);
            _languages = result;
        }
    }
    public static IReadOnlyList<string> Languages => _languages;
    public static string GetCurrentUserLanguage() => Thread.CurrentThread.CurrentCulture.IetfLanguageTag.Split('-').First() ?? "en";
    public static void SetCurrentUserLanguage(string lang)
    {
        var culture = new System.Globalization.CultureInfo(lang);
        Thread.CurrentThread.CurrentCulture = culture;
    }
    public static string GetNameOfLanguage(string lang)
        => lang switch
        {
            "en" => "English",
            "ru" => "Русский",
            _ => "чзх магия"
        };

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
