namespace ToyStore.Web.Models
{
    public class CartItem
    {
        public string ProductKey { get; set; }
        public string ProductName { get; set; } = "";
        public string Name { get; set; }
        public string Title { get; set; }
        public string Image { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
