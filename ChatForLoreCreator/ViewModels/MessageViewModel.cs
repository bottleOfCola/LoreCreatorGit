using System.ComponentModel.DataAnnotations;

namespace ChatForLoreCreator.ViweModel;

public class MessageViewModel
{
    [MinLength(1)]
    public string Text {  get; set; }
}
