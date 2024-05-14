using LoreCreator.DbStuff.Models;

namespace LoreCreator.ViewModels;

public class AddConnectionViewModel
{
    public string Description { get; set; }
    public int ConnectionTypeId { get; set; }
    public List<int> ElementsIds { get; set; }

    public List<ConnectionType> AllConnectionTypes { get; set; }
    public List<Element> AllElements { get; set; }
}
