using System.ComponentModel.DataAnnotations;

namespace FoodSelling.Models
{
    public class Food
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Required]
        [Range(0.01, 1000)]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string? Featured { get; set; }
        public string Active { get; set; } = "Active";
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public Category? Category { get; set; }
    }
}
