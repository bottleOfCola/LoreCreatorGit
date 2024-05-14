using SharedForLoreCreator.Models;

namespace LoreCreator.ViewModels;

public class ProfileViewModel
{
    public int Id {  get; set; }
    public string Name {  get; set; }
    public List<SharedRole> Roles { get; set; }
    public string Email { get; set; }
}
