using System.ComponentModel.DataAnnotations;

namespace DinnerSpinner.Domain
{
    public class Category
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
    }
}
