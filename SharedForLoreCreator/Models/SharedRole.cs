namespace SharedForLoreCreator.Models;

public class SharedRole : BaseModel
{
    public string Name { get; set; } = null!;

    public virtual List<SharedPermissionType> Permissions { get; set; } = null!;
}
