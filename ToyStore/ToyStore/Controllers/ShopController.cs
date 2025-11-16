using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using ToyStore.Web.Models;


namespace ToyStore.Web.Controllers
{
    public class ShopController : Controller
    {
        // Strona główna sklepu
        public IActionResult Index()
        {
            return View();
        }

        // Kategoria - każda kategoria ma swój widok
        public IActionResult Category(string name)
        {
            // mapowanie nazwy kategorii na widok
            var views = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["misie"] = "Bears",
                ["klocki"] = "CategoryKlocki",
                ["gry"] = "CategoryGry"
            };

            if (!views.ContainsKey(name))
                return NotFound();

            // przygotowanie produktów dla danej kategorii
            var products = new Dictionary<string, ProductModel>(StringComparer.OrdinalIgnoreCase);

            if (name.Equals("misie", StringComparison.OrdinalIgnoreCase))
            {
                products = new Dictionary<string, ProductModel>
                {
                    ["osmiornica"] = new ProductModel("Ośmiornica", "~/images/osmiornicablue.png",
                        new Dictionary<string, string> { { "blue", "osmiornicablue.png" }, { "pink", "osmiornicapink.png" }, { "yellow", "osmiornicyellow.png" }, { "purple", "plush_toy_octopus_purple.png" }, { "red", "plush_toy_octopus_red.png" } }),
                    ["kurczak"] = new ProductModel("Kurczak", "~/images/kurczakpink.png",
                        new Dictionary<string, string> { { "pink", "kurczakpink.png" }, { "purple", "kurczakpurple.png" }, { "blue", "plush_toy_chicken_blue.png" }, { "green", "plush_toy_chicken_green.png" }, { "grey", "plush_toy_chicken_grey.png" } }),
                    ["slon"] = new ProductModel("Słoń", "~/images/plush_toy_elephant_blue.png",
                        new Dictionary<string, string> { { "blue", "plush_toy_elephant_blue.png" }, { "grey", "plush_toy_elephant_grey.png" }, { "pink", "plush_toy_elephant_pink.png" } }),
                    ["foka"] = new ProductModel("Foka", "~/images/plush_toy_seal.jpg",
                        new Dictionary<string, string> { { "grey", "plush_toy_seal.jpg" } }),
                    ["pluszak"] = new ProductModel("Miś", "~/images/pluszak.jpg",
                        new Dictionary<string, string> { { "brown", "pluszak.jpg" } })
                };
            }
            else if (name.Equals("klocki", StringComparison.OrdinalIgnoreCase))
            {
                products = new Dictionary<string, ProductModel>
                {
                    ["lego_hogwarts_castle"] = new ProductModel("LEGO Hogwarts Castle", "~/images/lego_hogwarts_castle.jpg", null),
                    ["lego_greathall"] = new ProductModel("LEGO Great Hall", "~/images/greathall.jpg", null),
                    ["lego_dungeon"] = new ProductModel("LEGO Minecraft Dungeon", "~/images/lego_minecraft_dungeon.webp", null),
                    ["lego_irongolem"] = new ProductModel("LEGO Minecraft Iron Golem", "~/images/lego_minecraft_irongolem.jpg", null),
                    ["lego_village"] = new ProductModel("LEGO Minecraft Village", "~/images/lego_minecraft_village.jpg", null),
                    ["lego_police"] = new ProductModel("LEGO Policja z przyczepką", "~/images/legopolicja.jpg", null)
                };
            }
            else if (name.Equals("gry", StringComparison.OrdinalIgnoreCase))
            {
                products = new Dictionary<string, ProductModel>
                {
                    ["gra_memory"] = new ProductModel("Gra Memory", "~/images/memory.jpg", null),
                    ["puzzle_1000"] = new ProductModel("Puzzle 1000 elementów", "~/images/puzzle1000.jpg", null),
                    ["ukladanka_lego"] = new ProductModel("Układanka LEGO", "~/images/ukladanka_lego.jpg", null)
                };
            }

            ViewData["Products"] = products;

            return View(views[name]);
        }

        // Widok dla pojedynczego produktu
        public IActionResult Product(string name)
        {
            var products = new Dictionary<string, ProductModel>(StringComparer.OrdinalIgnoreCase)
            {
                // misie
                ["osmiornica"] = new ProductModel("Ośmiornica", "~/images/osmiornicablue.png",
                    new Dictionary<string, string> { { "blue", "osmiornicablue.png" }, { "pink", "osmiornicapink.png" }, { "yellow", "osmiornicyellow.png" }, { "purple", "plush_toy_octopus_purple.png" }, { "red", "plush_toy_octopus_red.png" } }),
                ["kurczak"] = new ProductModel("Kurczak", "~/images/kurczakpink.png",
                    new Dictionary<string, string> { { "pink", "kurczakpink.png" }, { "purple", "kurczakpurple.png" }, { "blue", "plush_toy_chicken_blue.png" }, { "green", "plush_toy_chicken_green.png" }, { "grey", "plush_toy_chicken_grey.png" } }),
                ["slon"] = new ProductModel("Słoń", "~/images/plush_toy_elephant_blue.png",
                    new Dictionary<string, string> { { "blue", "plush_toy_elephant_blue.png" }, { "grey", "plush_toy_elephant_grey.png" }, { "pink", "plush_toy_elephant_pink.png" } }),
                ["foka"] = new ProductModel("Foka", "~/images/plush_toy_seal.jpg",
                    new Dictionary<string, string> { { "grey", "plush_toy_seal.jpg" } }),
                ["pluszak"] = new ProductModel("Miś", "~/images/pluszak.jpg",
                    new Dictionary<string, string> { { "brown", "pluszak.jpg" } }),

                // klocki
                ["lego_hogwarts_castle"] = new ProductModel("LEGO Hogwarts Castle", "~/images/lego_hogwarts_castle.jpg", null),
                ["lego_greathall"] = new ProductModel("LEGO Great Hall", "~/images/greathall.jpg", null),
                ["lego_dungeon"] = new ProductModel("LEGO Minecraft Dungeon", "~/images/lego_minecraft_dungeon.webp", null),
                ["lego_irongolem"] = new ProductModel("LEGO Minecraft Iron Golem", "~/images/lego_minecraft_irongolem.jpg", null),
                ["lego_village"] = new ProductModel("LEGO Minecraft Village", "~/images/lego_minecraft_village.jpg", null),
                ["lego_police"] = new ProductModel("LEGO Policja z przyczepką", "~/images/legopolicja.jpg", null),

                // gry
                ["gra_memory"] = new ProductModel("Gra Memory", "~/images/memory.jpg", null),
                ["puzzle_1000"] = new ProductModel("Puzzle 1000 elementów", "~/images/puzzle1000.jpg", null),
                ["ukladanka_lego"] = new ProductModel("Układanka LEGO", "~/images/ukladanka_lego.jpg", null)
            };

            if (!products.ContainsKey(name))
                return NotFound();

            return View(products[name]);
        }
        [HttpPost]
        public IActionResult AddToCart(string name, string image, decimal price, int quantity = 1)
        {
            List<CartItem> cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            var existingItem = cart.FirstOrDefault(c => c.Name == name);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem { Name = name, Image = image, Price = price, Quantity = quantity });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Cart");
        }

        // Widok koszyka
        public IActionResult Cart()
        {
            List<CartItem> cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }
    }

    public record ProductModel(
        string Title,
        string DefaultImage,
        Dictionary<string, string>? Colors
    );
}
