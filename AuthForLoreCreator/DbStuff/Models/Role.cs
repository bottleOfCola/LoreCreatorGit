using SharedForLoreCreator.Models;
using System.Text.Json.Serialization;

namespace AuthForLoreCreator.DbStuff.Models;

public class Role : SharedRole
{

    public new List<PermissionType> Permissions { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public new List<User> Users { get; set; } = null!;
}
