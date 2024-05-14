using SharedForLoreCreator.Models;

namespace AuthForLoreCreator.DbStuff.Models;

public class User : SharedUser
{
    public string Password { get; set; } = null!;
    public new List<Role> Roles { get; set; } = null!;
}
