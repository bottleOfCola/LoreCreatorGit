using SharedForLoreCreator.Models;

namespace ChatForLoreCreator.DbStuff.Models;

public class Chat : BaseModel
{
    public string Name { get; set; }
    public List<Message> Messages { get; set; }
}
