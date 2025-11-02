using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
            // Strona kategorii "Misie"
            if (id == 2)
            {
                var bears = new List<dynamic>
                {
                    new { Name = "Miś Ośmiornica", Image = "/images/osmiornicapink.png", Link = "/Shop/Product?name=osmiornica" },
                    new { Name = "Miś Kurczak", Image = "/images/kurczakpink.png", Link = "/Shop/Product?name=kurczak" }
                };

                ViewBag.Bears = bears;
                return View("Bears");
            }

            return View();
        }

        public IActionResult Product(string name)
        {
            if (string.IsNullOrEmpty(name))
                return RedirectToAction("Index");

            dynamic product = new System.Dynamic.ExpandoObject();

            switch (name.ToLower())
            {
                case "osmiornica":
                    product.Name = "Miś Ośmiornica";
                    product.Description = "Uroczy pluszak ośmiornica – dostępny w trzech kolorach: żółty, różowy i niebieski.";
                    product.DefaultImage = "/images/osmiornicayellow.png";
                    product.Colors = new List<string> { "Żółty", "Różowy", "Niebieski" };
                    product.ColorImages = new Dictionary<string, string>
                    {
                        { "Żółty", "/images/osmiornicayellow.png" },
                        { "Różowy", "/images/osmiornicapink.png" },
                        { "Niebieski", "/images/osmiornicablue.png" }
                    };
                    break;

                case "kurczak":
                    product.Name = "Miś Kurczak";
                    product.Description = "Przytulny kurczaczek – dostępny w dwóch kolorach: różowy i fioletowy.";
                    product.DefaultImage = "/images/kurczakpink.png";
                    product.Colors = new List<string> { "Różowy", "Fioletowy" };
                    product.ColorImages = new Dictionary<string, string>
                    {
                        { "Różowy", "/images/kurczakpink.png" },
                        { "Fioletowy", "/images/kurczakpurple.png" }
                    };
                    break;

                default:
                    return RedirectToAction("Index");
            }

            ViewBag.Product = product;
            return View("Product");
        }
    }
}
