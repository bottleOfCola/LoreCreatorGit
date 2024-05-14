using AuthForLoreCreator.DbStuff.Models;

namespace AuthForLoreCreator.ViewModels;

public record UserViewModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
