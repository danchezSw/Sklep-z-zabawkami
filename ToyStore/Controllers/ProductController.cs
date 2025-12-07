using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToyStore.Data;
using ToyStore.Model.DataModels;

namespace ToyStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewData["Categories"] = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                    var path = Path.Combine(_env.WebRootPath, "images/products", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    model.DefaultImageUrl = "/images/products/" + fileName;
                }

                _context.Products.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var categories = await _context.Categories.ToListAsync();
            ViewData["Categories"] = new SelectList(categories, "Id", "Name", model.CategoryId);
            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            var categories = await _context.Categories.ToListAsync();
            ViewData["Categories"] = new SelectList(categories, "Id", "Name", product.CategoryId);

            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product model, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                var product = await _context.Products.FindAsync(model.Id);
                if (product == null) return NotFound();

                product.Name = model.Name;
                product.Description = model.Description;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;

                if (imageFile != null)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                    var path = Path.Combine(_env.WebRootPath, "images/products", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    product.DefaultImageUrl = "/images/products/" + fileName;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var categories = await _context.Categories.ToListAsync();
            ViewData["Categories"] = new SelectList(categories, "Id", "Name", model.CategoryId);

            return View(model);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ManageColors(int productId)
        {
            var product = await _context.Products
                .Include(p => p.ColorVariants)
                .FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null) return NotFound();

            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddColor(int productId, string color, decimal? price, IFormFile imageFile)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();

            var variant = new ProductColorVariant
            {
                ProductId = productId,
                Color = color,
                Price = price,
            };

            if (imageFile != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                var path = Path.Combine(_env.WebRootPath, "images/products", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                variant.ImageUrl = "/images/products/" + fileName;
            }

            _context.ProductColorVariants.Add(variant);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageColors), new { productId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteColor(int id)
        {
            var variant = await _context.ProductColorVariants.FindAsync(id);
            if (variant != null)
            {
                var productId = variant.ProductId;
                _context.ProductColorVariants.Remove(variant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageColors), new { productId });
            }

            return NotFound();
        }
    }
}
