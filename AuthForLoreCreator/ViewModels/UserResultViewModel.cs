using AuthForLoreCreator.DbStuff.Models;

namespace AuthForLoreCreator.ViewModels;

public record UserResultViewModel
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Email {  get; init; }
    public List<Role> Roles {  get; init; }

    public UserResultViewModel(User user)
    {
        Name = user.Name;
        Roles = user.Roles;
        Id = user.Id;
        Email = user.Email;
    }
}