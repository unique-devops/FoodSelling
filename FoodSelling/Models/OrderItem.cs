using System.ComponentModel.DataAnnotations;

namespace FoodSelling.Models
{
    public class OrderItem
    {
        [Key]
        public int orderItemId { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int FoodItemId { get; set; }

        public Food FoodItem { get; set; }
        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
