using Microsoft.AspNetCore.Mvc;
using ToyStore.Model.DataModels;
using ToyStore.Services;

namespace ToyStore.Web.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ReviewService _reviewService;

        public ReviewController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Review model)
        {
            if (!ModelState.IsValid)
            {
                
                TempData["ReviewError"] = "Wypełnij poprawnie wszystkie pola opinii.";
                return RedirectToAction("Details", "Shop", new { id = model.ProductId });
            }

            
            model.IsApproved = true;

            await _reviewService.AddAsync(model);

            TempData["ReviewSuccess"] = "Dziękujemy za opinię!";
            return RedirectToAction("Product", "Shop", new { id = model.ProductId });
        }
    }
}
