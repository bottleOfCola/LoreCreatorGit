using SharedForLoreCreator.Models;

namespace ChatForLoreCreator.DbStuff.Models;

public class Message : BaseModel
{
    public int UserId { get; set; }
    public string Text { get; set; }
    public DateTime DateTime { get; set; }
    public Chat Chat { get; set; }
}
