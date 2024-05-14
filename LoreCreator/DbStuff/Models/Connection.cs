using SharedForLoreCreator.Models;

namespace LoreCreator.DbStuff.Models;

public class Connection : BaseModel
{
    public string Description { get; set; }
    public ConnectionType ConnectionType { get; set; }
    public List<Element> Elements { get; set; }
}