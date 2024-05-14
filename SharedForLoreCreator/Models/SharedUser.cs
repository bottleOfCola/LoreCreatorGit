namespace SharedForLoreCreator.Models;

public class SharedUser : BaseModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public virtual List<SharedRole> Roles { get; set; } = null!;
}
