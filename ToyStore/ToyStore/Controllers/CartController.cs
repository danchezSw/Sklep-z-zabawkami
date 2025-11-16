using Microsoft.AspNetCore.Mvc;
using ToyStore.Web.Models;
using System.Collections.Generic;

namespace ToyStore.Web.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            // Pobierz koszyk z sesji
            var cartItems = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart")
                            ?? new List<CartItem>();

            ViewBag.CartCount = cartItems.Sum(x => x.Quantity);

            return View(cartItems);
        }
    }
}
