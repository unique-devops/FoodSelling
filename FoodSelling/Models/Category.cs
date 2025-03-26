using System.ComponentModel.DataAnnotations;

namespace FoodSelling.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public string? Featured { get; set; }
        public string? Active { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public ICollection<Food>? FoodItems { get; set; }
    }
}
