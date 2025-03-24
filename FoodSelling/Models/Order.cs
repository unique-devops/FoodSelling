namespace FoodSelling.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Total_Price { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public List<OrderItem> Items { get; set; }
    } 
}
