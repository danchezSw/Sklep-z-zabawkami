using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyStore.Data;
using ToyStore.Model.DataModels;

[Authorize(Roles = "Admin")]
public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public CategoryController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories
                                   .Include(c => c.Products)
                                   .ToListAsync();
        return View(categories);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category, IFormFile ImageFile)
    {
        if (ModelState.IsValid)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var ext = Path.GetExtension(ImageFile.FileName).ToLower();
                var allowedExt = new[] { ".jpg", ".jpeg", ".png" };
                if (!allowedExt.Contains(ext))
                {
                    ModelState.AddModelError("ImageFile", "Dozwolone formaty: JPG, JPEG, PNG.");
                    return View(category);
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "images/categories");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + ext;
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                category.ImageUrl = fileName;
            }

            _context.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(category);
    }


    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category category, IFormFile? ImageFile)
    {
        if (id != category.Id) return NotFound();

        var dbCategory = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (dbCategory == null) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var ext = Path.GetExtension(ImageFile.FileName).ToLower();
                    var allowedExt = new[] { ".jpg", ".jpeg", ".png" };
                    if (!allowedExt.Contains(ext))
                    {
                        ModelState.AddModelError("ImageFile", "Dozwolone formaty: JPG, JPEG, PNG.");
                        return View(category);
                    }

                    var uploadsFolder = Path.Combine(_env.WebRootPath, "images/categories");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    if (!string.IsNullOrEmpty(dbCategory.ImageUrl))
                    {
                        var oldPath = Path.Combine(uploadsFolder, dbCategory.ImageUrl);
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    var newFile = Guid.NewGuid() + ext;
                    var newPath = Path.Combine(uploadsFolder, newFile);

                    using (var stream = new FileStream(newPath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    category.ImageUrl = newFile;
                }
                else
                {
                    category.ImageUrl = dbCategory.ImageUrl;
                }

                _context.Update(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Categories.Any(e => e.Id == id)) return NotFound();
                else throw;
            }
        }

        return View(category);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category != null)
        {
            if (!string.IsNullOrEmpty(category.ImageUrl))
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "images/categories");
                var imgPath = Path.Combine(uploadsFolder, category.ImageUrl);
                if (System.IO.File.Exists(imgPath))
                    System.IO.File.Delete(imgPath);
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
