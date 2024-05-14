using System.ComponentModel.DataAnnotations;

namespace LoreCreator.ViewModels;

public class AddElementViewModel
{
    [MinLength(2, ErrorMessage = "Должно быть не менее 2х символов")]
    [MaxLength(35, ErrorMessage = "Должно быть не более 35и символов")]
    public string Name { get; set; }

    [MinLength(10, ErrorMessage = "Должно быть не менее 10и символов")]
    public string Description { get; set; }
    public string Image { get; set; }
    public List<int> Tags { get; set; }
}
