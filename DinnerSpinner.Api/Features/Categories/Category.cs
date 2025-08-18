using System.ComponentModel.DataAnnotations;

namespace DinnerSpinner.Api.Features.Categories
{
    public class Category
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
