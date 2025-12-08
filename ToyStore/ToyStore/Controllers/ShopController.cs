using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyStore.Data;
using ToyStore.Model.DataModels;
using ToyStore.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Alias dla CartItem z Web.Models
using WebCartItem = ToyStore.Web.Models.CartItem;

namespace ToyStore.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Strona główna sklepu - lista kategorii i produktów
        public async Task<IActionResult> Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<WebCartItem>>("Cart")
                       ?? new List<WebCartItem>();
            ViewBag.CartCount = cart.Sum(x => x.Quantity);

            var categories = await _context.Categories
                                .Include(c => c.Products)
                                .ToListAsync();

            return View(categories);
        }

        // Widok kategorii po ID
        public async Task<IActionResult> Category(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories
                .Include(c => c.Products)
                .ThenInclude(p => p.ColorVariants)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return NotFound();

            return View(category);
        }

        // Widok pojedynczego produktu
        public async Task<IActionResult> Product(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ColorVariants)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1, int? colorVariantId = null)
        {
            var product = await _context.Products
                .Include(p => p.ColorVariants)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null) return NotFound();

            // Wydzielamy wybrany wariant, jeśli istnieje
            var selectedVariant = colorVariantId != null
                                  ? product.ColorVariants.FirstOrDefault(c => c.Id == colorVariantId)
                                  : null;

            string image = selectedVariant?.ImageUrl ?? product.DefaultImageUrl;
            decimal price = selectedVariant?.Price ?? product.Price;
            string color = selectedVariant?.Color; // zapis koloru

            var cart = HttpContext.Session.GetObjectFromJson<List<WebCartItem>>("Cart")
                       ?? new List<WebCartItem>();

            var existing = cart.FirstOrDefault(c => c.ProductKey == $"{productId}_{colorVariantId}");
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                cart.Add(new WebCartItem
                {
                    ProductKey = $"{productId}_{colorVariantId}",
                    ProductName = product.Name,
                    Image = image,
                    Price = price,
                    Quantity = quantity,
                    Color = color
                });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return RedirectToAction("Index", "Cart");
        }

        // Widok koszyka
        public IActionResult Cart()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<WebCartItem>>("Cart")
                       ?? new List<WebCartItem>();
            return View(cart);
        }

        // Usuwanie produktu z koszyka
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromCart(string productKey)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<WebCartItem>>("Cart")
                       ?? new List<WebCartItem>();

            var item = cart.FirstOrDefault(c => c.ProductKey == productKey);
            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            return RedirectToAction("Index", "Cart");
        }
    }
}
