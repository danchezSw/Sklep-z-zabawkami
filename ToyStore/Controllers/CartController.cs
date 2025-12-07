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
        public IActionResult Remove(string productKey)
        {
            var cartItems = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart")
                            ?? new List<CartItem>();

            var itemToRemove = cartItems.FirstOrDefault(x => x.ProductKey == productKey);
            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
                HttpContext.Session.SetObjectAsJson("Cart", cartItems); // zapis koszyka
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult UpdateQuantity(string productKey, int quantity)
        {
            var cartItems = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var item = cartItems.FirstOrDefault(x => x.ProductKey == productKey);
            if (item != null)
            {
                item.Quantity = quantity; // aktualizacja ilości
                HttpContext.Session.SetObjectAsJson("Cart", cartItems); // zapis do sesji
            }

            return RedirectToAction("Index");
        }

    }

}
