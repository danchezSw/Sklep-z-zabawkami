namespace ToyStore.Web.Models
{
    public class WishlistItem
    {
        public int ProductId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
