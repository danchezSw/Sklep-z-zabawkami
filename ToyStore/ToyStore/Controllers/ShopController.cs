using Microsoft.AspNetCore.Mvc;
using System;
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
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            ViewBag.CartCount = cart.Sum(x => x.Quantity);
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
                ["gry"] = "CategoryGry",
                ["barbie"] = "CategoryBarbie",
                ["hotwheels"] = "CategoryHotWheels"
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
                        new Dictionary<string, string> {
                            { "blue", "osmiornicablue.png" },
                            { "pink", "osmiornicapink.png" },
                            { "yellow", "osmiornicyellow.png" },
                            { "purple", "plush_toy_octopus_purple.png" },
                            { "red", "plush_toy_octopus_red.png" }
                        },
                        49.99M
                    ),
                    ["kurczak"] = new ProductModel("Kurczak", "~/images/kurczakpink.png",
                        new Dictionary<string, string> {
                            { "pink", "kurczakpink.png" },
                            { "purple", "kurczakpurple.png" },
                            { "blue", "plush_toy_chicken_blue.png" },
                            { "green", "plush_toy_chicken_green.png" },
                            { "grey", "plush_toy_chicken_grey.png" }
                        },
                        39.99M
                    ),
                    ["slon"] = new ProductModel("Słoń", "~/images/plush_toy_elephant_blue.png",
                        new Dictionary<string, string> {
                            { "blue", "plush_toy_elephant_blue.png" },
                            { "grey", "plush_toy_elephant_grey.png" },
                            { "pink", "plush_toy_elephant_pink.png" }
                        },
                        45.00M
                    ),
                    ["foka"] = new ProductModel("Foka", "~/images/plush_toy_seal.jpg",
                        new Dictionary<string, string> { { "grey", "plush_toy_seal.jpg" } },
                        29.99M
                    ),
                    ["pluszak"] = new ProductModel("Miś", "~/images/pluszak.jpg",
                        new Dictionary<string, string> { { "brown", "pluszak.jpg" } },
                        24.99M
                    ),
                    ["krolik"] = new ProductModel("Królik", "~/images/krolik_blue.png",
            new Dictionary<string, string> {
                { "blue", "krolik_blue.png" },
                { "pink", "krolik_pink.png" },
                { "white", "krolik_white.png" }
            }, 34.99M
        ),
                };
            }
            else if (name.Equals("klocki", StringComparison.OrdinalIgnoreCase))
            {
                products = new Dictionary<string, ProductModel>(StringComparer.OrdinalIgnoreCase)
                {
                    ["lego_duplo_10986"] = new ProductModel("LEGO Duplo 10986", "lego_duplo_10986.jpg", null, 129.99M),
                    ["lego_duplo_10990"] = new ProductModel("LEGO Duplo 10990", "lego_duplo_10990.jpg", null, 119.99M),
                    ["lego_duplo_10991"] = new ProductModel("LEGO Duplo 10991", "lego_duplo_10991.webp", null, 139.99M),
                    ["lego_duplo_myFirstCarsAndTrucks"] = new ProductModel("LEGO Duplo My First Cars and Trucks", "lego_duplo_myFirstCarsAndTrucks.jpg", null, 99.99M),
                    ["lego_hogwarts_castle"] = new ProductModel("LEGO Hogwarts Castle", "lego_hogwarts_castle.jpg", null, 299.99M),
                    ["lego_hogwarts_greathall"] = new ProductModel("LEGO Great Hall", "greathall.jpg", null, 199.99M),
                    ["lego_minecraft_dungeon"] = new ProductModel("LEGO Minecraft Dungeon", "lego_minecraft_dungeon.webp", null, 149.99M),
                    ["lego_minecraft_irongolem"] = new ProductModel("LEGO Minecraft Iron Golem", "lego_minecraft_irongolem.jpg", null, 129.99M),
                    ["lego_minecraft_village"] = new ProductModel("LEGO Minecraft Village", "lego_minecraft_village.jpg", null, 179.99M),
                    ["lego_onepiece_BuggyCircusTent"] = new ProductModel("LEGO One Piece Buggy Circus Tent", "lego_onepiece_BuggyCircusTent.jpg", null, 149.99M),
                    ["lego_onepiece_MonkeyDLuffy"] = new ProductModel("LEGO One Piece Monkey D. Luffy", "lego_onepiece_MonkeyDLuffy.jpg", null, 149.99M),
                    ["lego_onepiece_battleAtArlongPark"] = new ProductModel("LEGO One Piece Battle at Arlong Park", "lego_onepiece_battleAtArlongPark.png", null, 149.99M),
                    ["lego_onepiece_goingmerry"] = new ProductModel("LEGO One Piece Going Merry", "lego_onepiece_goingmerry.jpg", null, 129.99M),
                    ["lego_policja_samochod_przyczepka"] = new ProductModel("LEGO Policja samochód z przyczepką", "lego_policja_samochod_przyczepka.jpg", null, 89.99M),
                    ["lego_starwars_assaultonhoth"] = new ProductModel("LEGO Star Wars Assault on Hoth", "lego_starwars_assaultonhoth.jpg", null, 199.99M),
                    ["lego_starwars_atap_walker"] = new ProductModel("LEGO Star Wars AT-AP Walker", "lego_starwars_atap_walker.jpg", null, 179.99M),
                    ["lego_starwars_deathstar"] = new ProductModel("LEGO Star Wars Death Star", "lego_starwars_deathstar.jpg", null, 399.99M),
                    ["lego_starwars_fo_snowspeeder"] = new ProductModel("LEGO Star Wars FO Snowspeeder", "lego_starwars_fo_snowspeeder.jpg", null, 149.99M),
                    ["lego_starwars_fo_transporter"] = new ProductModel("LEGO Star Wars FO Transporter", "lego_starwars_fo_transporter.jpg", null, 179.99M),
                    ["lego_starwars_jedi_defenderclasscruiser"] = new ProductModel("LEGO Star Wars Jedi Defender-class Cruiser", "lego_starwars_jedi_defenderclasscruiser.jpg", null, 199.99M),
                    ["lego_starwars_milleniumfalcon"] = new ProductModel("LEGO Star Wars Millennium Falcon", "lego_starwars_milleniumfalcon.jpg", null, 399.99M),
                    ["lego_starwars_republicdropship"] = new ProductModel("LEGO Star Wars Republic Dropship", "lego_starwars_republicdropship.jpg", null, 149.99M),
                    ["lego_starwars_sandcrawler"] = new ProductModel("LEGO Star Wars Sandcrawler", "lego_starwars_sandcrawler.jpg", null, 299.99M)
                };
            }




            else if (name.Equals("gry", StringComparison.OrdinalIgnoreCase))
            {
                products = new Dictionary<string, ProductModel>
                {
                    ["gra_memory"] = new ProductModel("Gra Memory", "~/images/Gra-memory.jpg", null, 19.99M),
                    ["puzzle_1000"] = new ProductModel("Puzzle 1000 elementów", "~/images/puzzle.jpg", null, 49.99M),
                };

            }
            else if (name.Equals("barbie", StringComparison.OrdinalIgnoreCase))
            {
                products = new Dictionary<string, ProductModel>(StringComparer.OrdinalIgnoreCase)
                {
                    ["barbieblond"] = new ProductModel("Barbie Blond", "~/images/barbieblond.jpg", null, 59.99M),
                    ["barbieblack"] = new ProductModel("Barbie Black", "~/images/barbieblack.jpg", null, 59.99M),
                    ["barbiepink"] = new ProductModel("Barbie Pink", "~/images/barbiepink.jpg", null, 59.99M)
                };
            }
            else if (name.Equals("hotwheels", StringComparison.OrdinalIgnoreCase))
            {
                products = new Dictionary<string, ProductModel>(StringComparer.OrdinalIgnoreCase)
                {
                    ["hotwheels1"] = new ProductModel("Hot Wheels 1", "~/images/hotwheels1.jpg", null, 29.99M),
                    ["hotwheels2"] = new ProductModel("Hot Wheels 2", "~/images/hotwheels2.jpg", null, 29.99M)
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
                    new Dictionary<string, string> {
                        { "blue", "osmiornicablue.png" },
                        { "pink", "osmiornicapink.png" },
                        { "yellow", "osmiornicyellow.png" },
                        { "purple", "plush_toy_octopus_purple.png" },
                        { "red", "plush_toy_octopus_red.png" }
                    },
                    49.99M
                ),
                ["kurczak"] = new ProductModel("Kurczak", "~/images/kurczakpink.png",
                    new Dictionary<string, string> {
                        { "pink", "kurczakpink.png" },
                        { "purple", "kurczakpurple.png" },
                        { "blue", "plush_toy_chicken_blue.png" },
                        { "green", "plush_toy_chicken_green.png" },
                        { "grey", "plush_toy_chicken_grey.png" }
                    },
                    39.99M
                ),
                ["slon"] = new ProductModel("Słoń", "~/images/plush_toy_elephant_blue.png",
                    new Dictionary<string, string> {
                        { "blue", "plush_toy_elephant_blue.png" },
                        { "grey", "plush_toy_elephant_grey.png" },
                        { "pink", "plush_toy_elephant_pink.png" }
                    },
                    45.00M
                ),
                ["foka"] = new ProductModel("Foka", "~/images/plush_toy_seal.jpg",
                    new Dictionary<string, string> { { "grey", "plush_toy_seal.jpg" } },
                    29.99M
                ),
                ["pluszak"] = new ProductModel("Miś", "~/images/pluszak.jpg",
                    new Dictionary<string, string> { { "brown", "pluszak.jpg" } },
                    24.99M
                ),
                ["krolik"] = new ProductModel("Królik", "~/images/krolik_blue.png",
            new Dictionary<string, string> {
                { "blue", "krolik_blue.png" },
                { "pink", "krolik_pink.png" },
                { "white", "krolik_white.png" }
            }, 34.99M
        ),

                // klocki
                ["lego_duplo"] = new ProductModel("LEGO Duplo", "~/images/lego_duplo.avif", null, 149.99M),
                ["lego_duplo_10986"] = new ProductModel("LEGO Duplo 10986", "~/images/lego_duplo_10986.jpg", null, 129.99M),
                ["lego_duplo_10990"] = new ProductModel("LEGO Duplo 10990", "~/images/lego_duplo_10990.jpg", null, 119.99M),
                ["lego_duplo_10991"] = new ProductModel("LEGO Duplo 10991", "~/images/lego_duplo_10991.webp", null, 139.99M),
                ["lego_duplo_myFirstCarsAndTrucks"] = new ProductModel("LEGO Duplo My First Cars and Trucks", "~/images/lego_duplo_myFirstCarsAndTrucks.jpg", null, 99.99M),
                ["lego_hogwarts_castle"] = new ProductModel("LEGO Hogwarts Castle", "~/images/lego_hogwarts_castle.jpg", null, 299.99M),
                ["lego_hogwarts_greathall"] = new ProductModel("LEGO Great Hall", "~/images/lego_hogwarts_greathall.avif", null, 199.99M),
                ["lego_minecraft_dungeon"] = new ProductModel("LEGO Minecraft Dungeon", "~/images/lego_minecraft_dungeon.webp", null, 149.99M),
                ["lego_minecraft_irongolem"] = new ProductModel("LEGO Minecraft Iron Golem", "~/images/lego_minecraft_irongolem.jpg", null, 129.99M),
                ["lego_minecraft_village"] = new ProductModel("LEGO Minecraft Village", "~/images/lego_minecraft_village.jpg", null, 179.99M),
                ["lego_onepiece_BuggyCircusTent"] = new ProductModel("LEGO One Piece Buggy Circus Tent", "~/images/lego_onepiece_BuggyCircusTent.jpg", null, 149.99M),
                ["lego_onepiece_MonkeyDLuffy"] = new ProductModel("LEGO One Piece Monkey D. Luffy", "~/images/lego_onepiece_MonkeyDLuffy.jpg", null, 149.99M),
                ["lego_onepiece_battleAtArlongPark"] = new ProductModel("LEGO One Piece Battle at Arlong Park", "~/images/lego_onepiece_battleAtArlongPark.png", null, 149.99M),
                ["lego_onepiece_goingmerry"] = new ProductModel("LEGO One Piece Going Merry", "~/images/lego_onepiece_goingmerry.jpg", null, 129.99M),
                ["lego_policja_samochod_przyczepka"] = new ProductModel("LEGO Policja samochód z przyczepką", "~/images/lego_policja_samochod_przyczepka.jpg", null, 89.99M),
                ["lego_starwars_assaultonhoth"] = new ProductModel("LEGO Star Wars Assault on Hoth", "~/images/lego_starwars_assaultonhoth.jpg", null, 199.99M),
                ["lego_starwars_atap_walker"] = new ProductModel("LEGO Star Wars AT-AP Walker", "~/images/lego_starwars_atap_walker.jpg", null, 179.99M),
                ["lego_starwars_deathstar"] = new ProductModel("LEGO Star Wars Death Star", "~/images/lego_starwars_deathstar.jpg", null, 399.99M),
                ["lego_starwars_fo_snowspeeder"] = new ProductModel("LEGO Star Wars FO Snowspeeder", "~/images/lego_starwars_fo_snowspeeder.jpg", null, 149.99M),
                ["lego_starwars_fo_transporter"] = new ProductModel("LEGO Star Wars FO Transporter", "~/images/lego_starwars_fo_transporter.jpg", null, 179.99M),
                ["lego_starwars_jedi_defenderclasscruiser"] = new ProductModel("LEGO Star Wars Jedi Defender-class Cruiser", "~/images/lego_starwars_jedi_defenderclasscruiser.jpg", null, 199.99M),
                ["lego_starwars_milleniumfalcon"] = new ProductModel("LEGO Star Wars Millennium Falcon", "~/images/lego_starwars_milleniumfalcon.jpg", null, 399.99M),
                ["lego_starwars_republicdropship"] = new ProductModel("LEGO Star Wars Republic Dropship", "~/images/lego_starwars_republicdropship.jpg", null, 149.99M),
                ["lego_starwars_sandcrawler"] = new ProductModel("LEGO Star Wars Sandcrawler", "~/images/lego_starwars_sandcrawler.jpg", null, 299.99M),

                // gry
                ["gra_memory"] = new ProductModel("Gra Memory", "~/images/Gra-memory.jpg", null, 19.99M),
                ["puzzle_1000"] = new ProductModel("Puzzle 1000 elementów", "~/images/puzzle.jpg", null, 49.99M),
                // Barbie – dodaj wszystkie trzy
                ["barbieblond"] = new ProductModel("Barbie Blond", "~/images/barbieblond.jpg", null, 59.99M),
                ["barbieblack"] = new ProductModel("Barbie Black", "~/images/barbieblack.jpg", null, 59.99M),
                ["barbiepink"] = new ProductModel("Barbie Pink", "~/images/barbiepink.jpg", null, 59.99M),

                // Hot Wheels – dodaj wszystkie
                ["hotwheels1"] = new ProductModel("Hot Wheels 1", "~/images/hotwheels1.jpg", null, 29.99M),
                ["hotwheels2"] = new ProductModel("Hot Wheels 2", "~/images/hotwheels2.jpg", null, 29.99M)

            };

            if (!products.ContainsKey(name))
                return NotFound();

            return View(products[name]);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(string name, string image, decimal price, int quantity = 1)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart")
                       ?? new List<CartItem>();

            var existing = cart.FirstOrDefault(c => c.ProductName == name);

            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductName = name,
                    Image = image,
                    Price = price,
                    Quantity = quantity
                });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return RedirectToAction("Index", "Cart");
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
        Dictionary<string, string>? Colors,
        decimal Price
    );
}
