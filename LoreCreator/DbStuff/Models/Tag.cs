using SharedForLoreCreator.Models;

namespace LoreCreator.DbStuff.Models;

public class Tag : BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Element> Elements { get; set; }
}
