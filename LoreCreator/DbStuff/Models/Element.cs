using SharedForLoreCreator.Models;

namespace LoreCreator.DbStuff.Models;

public class Element : BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string? Image { get; set; }
    public List<Tag> Tags { get; set; }
    public List<Connection> Connections { get; set; }
}
