using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToyStore.Models;
using ToyStore.Data;               // Twój ApplicationDbContext
using ToyStore.Web.Models;         // HomeProductModel
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ToyStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // Pobierz 4 najnowsze produkty z bazy
            var newProducts = _context.Products
                .OrderByDescending(p => p.Id)
                .Take(4)
                .Select(p => new HomeProductModel
                {
                    Id = p.Id,
                    Title = p.Name,
                    Image = p.DefaultImageUrl
                })
                .ToList();

            ViewData["NewProducts"] = newProducts;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
