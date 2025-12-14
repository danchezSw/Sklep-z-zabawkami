using System.ComponentModel.DataAnnotations;

namespace ToyStore.ViewModels.VM
{
    public class CheckoutVm
    {
        [Required]
        public string CustomerName { get; set; }

        [Required, EmailAddress]
        public string CustomerEmail { get; set; }

        [Required]
        public string ShippingAddress { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        public List<CartItemVm> CartItems { get; set; } = new();
    }
}
