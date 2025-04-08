using System.ComponentModel.DataAnnotations;

namespace FoodSelling.Models
{
    public class Order
    {
        [Key]

        public int OrderId { get; set; }
        public string? CustomerName { get; set; }

        [Required]
        public string CustomerEmail { get; set; }
        public decimal Quantity { get; set; }        
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public ICollection<OrderItem> OrderItems { get; set; }
    } 
}
