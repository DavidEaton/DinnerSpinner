using DinnerSpinner.Domain.Features.Categories;

namespace DinnerSpinner.Domain.Features.Dishes;

public class Dish
{
    public int Id { get; set; }

    // [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    public string Name { get; set; } = string.Empty;

    public Category Category { get; set; } = new Category();
}
