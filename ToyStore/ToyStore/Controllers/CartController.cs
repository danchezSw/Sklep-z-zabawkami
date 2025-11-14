using Microsoft.AspNetCore.Mvc;
using ToyStore.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace ToyStore.Web.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            // Pobierz koszyk z LocalStorage lub sesji — tutaj dla przykładu pusta lista
            var cartItems = new List<CartItem>();

            // Przykładowe dane jeśli chcesz testować
            // cartItems.Add(new CartItem { ProductName = "LEGO Hogwarts", Quantity = 1, Price = 299.99M, Image = "~/images/lego_hogwarts_castle.jpg" });

            return View(cartItems);
        }
    }
}
