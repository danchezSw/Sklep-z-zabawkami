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
        public IActionResult AddToCart(int productId, int? colorVariantId, int quantity = 1)
        {
            if (productId <= 0) return BadRequest("Nieprawidłowy produkt.");

            var product = _dbContext.Products.Find(productId);
            if (product == null) return NotFound();

            string? color = null;
            decimal price = product.Price;
            string? image = product.DefaultImageUrl;

            if (colorVariantId.HasValue)
            {
                var variant = _dbContext.ProductColorVariants.Find(colorVariantId.Value);
                if (variant != null)
                {
                    color = variant.Color;
                    price = variant.Price ?? price;
                    image = variant.ImageUrl ?? image;
                    // NIE nadpisujemy productId!
                }
            }

            var cartItems = HttpContext.Session.GetObjectFromJson<List<WebCartItem>>("Cart")
                            ?? new List<WebCartItem>();

            // Unikalny klucz dla każdego produktu + wariantu
            string productKey = $"{productId}_{colorVariantId ?? 0}";

            var existingItem = cartItems.FirstOrDefault(c => c.ProductKey == productKey);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cartItems.Add(new WebCartItem
                {
                    ProductKey = productKey,
                    ProductId = productId,
                    ProductName = product.Name,
                    Image = image ?? "",
                    Quantity = quantity,
                    Price = price,
                    Color = color
                });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cartItems);

            return RedirectToAction("Index", "Cart");
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
