namespace ToyStore.Web.Models
{
    public class CartItem
    {
        public string ProductKey { get; set; } = "";
        public int ProductId { get; set; }

        public string ProductName { get; set; } = "";
        public string Image { get; set; } = "";
        public string? Color { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
