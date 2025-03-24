namespace FoodSelling.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string? Featured { get; set; }
        public string Active { get; set; }
        public string? ImageUrl { get; set; }
    }
}
