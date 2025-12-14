using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ToyStore.Data;
using ToyStore.Model.DataModels;

using WebCartItem = ToyStore.Web.Models.CartItem;

namespace ToyStore.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CartController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var cartItems = HttpContext.Session.GetObjectFromJson<List<WebCartItem>>("Cart")
                            ?? new List<WebCartItem>();

            ViewBag.CartCount = cartItems.Sum(x => x.Quantity);

            return View(cartItems);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1, string? color = null)
        {
            var product = _dbContext.Products.Find(productId);
            if (product == null) return NotFound();

            var cartItems = HttpContext.Session.GetObjectFromJson<List<WebCartItem>>("Cart")
                            ?? new List<WebCartItem>();

            var existingItem = cartItems.FirstOrDefault(c => c.ProductId == product.Id && c.Color == color);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cartItems.Add(new WebCartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Image = product.DefaultImageUrl ?? "",
                    Quantity = quantity,
                    Price = product.Price,
                    Color = color,
                    ProductKey = Guid.NewGuid().ToString()
                });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cartItems);

            return RedirectToAction("Index", "Shop");
        }

        [HttpPost]
        public IActionResult Remove(string productKey)
        {
            var cartItems = HttpContext.Session.GetObjectFromJson<List<WebCartItem>>("Cart")
                            ?? new List<WebCartItem>();

            var itemToRemove = cartItems.FirstOrDefault(x => x.ProductKey == productKey);
            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
                HttpContext.Session.SetObjectAsJson("Cart", cartItems);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(string productKey, int quantity)
        {
            var cartItems = HttpContext.Session.GetObjectFromJson<List<WebCartItem>>("Cart")
                            ?? new List<WebCartItem>();

            var item = cartItems.FirstOrDefault(x => x.ProductKey == productKey);
            if (item != null)
            {
                item.Quantity = quantity;
                HttpContext.Session.SetObjectAsJson("Cart", cartItems);
            }

            return RedirectToAction("Index");
        }
    }
}
