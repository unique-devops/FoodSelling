namespace FoodSelling.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int FoodId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }        
        public decimal TotalPrice { get; set; }
    }
}
