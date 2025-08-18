using DinnerSpinner.Api.Features.Categories;
using System.ComponentModel.DataAnnotations;

namespace DinnerSpinner.Api.Features.Dishes
{
    public class Dish
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Category Category { get; set; } = new Category();
    }
}
