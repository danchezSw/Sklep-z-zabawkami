using Microsoft.AspNetCore.Mvc;

namespace ToyStore.Web.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            // w przyszłości można dodać pobieranie szczegółów produktu po ID
            return View();
        }
    }
}