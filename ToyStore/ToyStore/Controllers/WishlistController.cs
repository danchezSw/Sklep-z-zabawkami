using Microsoft.AspNetCore.Mvc;
using ToyStore.DAL;
using ToyStore.Data;
using ToyStore.Web.Models;

namespace ToyStore.Web.Controllers
{
    public class WishlistController : Controller
    {
        private readonly ApplicationDbContext _db;

        public WishlistController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var wishlist = HttpContext.Session
                .GetObjectFromJson<List<WishlistItem>>("Wishlist")
                ?? new List<WishlistItem>();

            return View(wishlist);
        }

        [HttpPost]
        public IActionResult Add(int productId)
        {
            var wishlist = HttpContext.Session
                .GetObjectFromJson<List<WishlistItem>>("Wishlist")
                ?? new List<WishlistItem>();

            if (!wishlist.Any(x => x.ProductId == productId))
            {
                var product = _db.Products.FirstOrDefault(p => p.Id == productId);

                if (product != null)
                {
                    wishlist.Add(new WishlistItem
                    {
                        ProductId = product.Id,
                        Title = product.Name,
                        Price = product.Price,
                    });
                }
            }

            HttpContext.Session.SetObjectAsJson("Wishlist", wishlist);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int productId)
        {
            var wishlist = HttpContext.Session
                .GetObjectFromJson<List<WishlistItem>>("Wishlist")
                ?? new List<WishlistItem>();

            wishlist.RemoveAll(x => x.ProductId == productId);

            HttpContext.Session.SetObjectAsJson("Wishlist", wishlist);
            return RedirectToAction("Index");
        }
    }
}
