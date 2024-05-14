using SharedForLoreCreator.Models;
using System.Text.Json.Serialization;

namespace AuthForLoreCreator.DbStuff.Models;

public class PermissionType : SharedPermissionType
{
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public new List<Role> Roles {  get; set; } = null!;
}