using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToyStore.Models;
using System.Collections.Generic;
using System.Linq;


namespace ToyStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Model dla produkt�w wy�wietlanych na stronie g��wnej
        public class HomeProductModel
        {
            public string ?Key { get; set; }      // np. "gra_memory"
            public string ?Title { get; set; }    // np. "Gra Memory"
            public string ?Image { get; set; }    // np. "~/images/Gra-memory.jpg"
        }

        public IActionResult Index()
        {
            // Lista wszystkich produkt�w z ca�ego sklepu
            var allProducts = new List<HomeProductModel>
            {
                // misie
                new HomeProductModel { Key = "osmiornica", Title = "Ośmiornica", Image = "~/images/osmiornicablue.png" },
                new HomeProductModel { Key = "kurczak", Title = "Kurczak", Image = "~/images/kurczakpink.png" },
                new HomeProductModel { Key = "slon", Title = "Słoń", Image = "~/images/plush_toy_elephant_blue.png" },
                new HomeProductModel { Key = "foka", Title = "Foka", Image = "~/images/plush_toy_seal.jpg" },
                new HomeProductModel { Key = "pluszak", Title = "Miś", Image = "~/images/pluszak.jpg" },
                new HomeProductModel { Key = "krolik", Title = "Królik", Image = "~/images/krolik_blue.png"},

                // klocki
                new HomeProductModel { Key = "lego_duplo_10986", Title = "LEGO Duplo 10986", Image = "~/images/lego_duplo_10986.jpg" },
                new HomeProductModel { Key = "lego_duplo_10990", Title = "LEGO Duplo 10990", Image = "~/images/lego_duplo_10990.jpg" },
                new HomeProductModel { Key = "lego_duplo_10991", Title = "LEGO Duplo 10991", Image = "~/images/lego_duplo_10991.webp" },
                new HomeProductModel { Key = "lego_duplo_myFirstCarsAndTrucks", Title = "LEGO Duplo My First Cars and Trucks", Image = "~/images/lego_duplo_myFirstCarsAndTrucks.jpg" },
                new HomeProductModel { Key = "lego_hogwarts_castle", Title = "LEGO Hogwarts Castle", Image = "~/images/lego_hogwarts_castle.jpg" },
                new HomeProductModel { Key = "lego_hogwarts_greathall", Title = "LEGO Great Hall", Image = "~/images/greathall.jpg" },
                new HomeProductModel { Key = "lego_minecraft_dungeon", Title = "LEGO Minecraft Dungeon", Image = "~/images/lego_minecraft_dungeon.webp" },
                new HomeProductModel { Key = "lego_minecraft_irongolem", Title = "LEGO Minecraft Iron Golem", Image = "~/images/lego_minecraft_irongolem.jpg" },
                new HomeProductModel { Key = "lego_minecraft_village", Title = "LEGO Minecraft Village", Image = "~/images/lego_minecraft_village.jpg" },
                new HomeProductModel { Key = "lego_onepiece_BuggyCircusTent", Title = "LEGO One Piece Buggy Circus Tent", Image = "~/images/lego_onepiece_BuggyCircusTent.jpg" },
                new HomeProductModel { Key = "lego_onepiece_MonkeyDLuffy", Title = "LEGO One Piece Monkey D. Luffy", Image = "~/images/lego_onepiece_MonkeyDLuffy.jpg" },
                new HomeProductModel { Key = "lego_onepiece_battleAtArlongPark", Title = "LEGO One Piece Battle at Arlong Park", Image = "~/images/lego_onepiece_battleAtArlongPark.png" },
                new HomeProductModel { Key = "lego_onepiece_goingmerry", Title = "LEGO One Piece Going Merry", Image = "~/images/lego_onepiece_goingmerry.jpg" },
                new HomeProductModel { Key = "lego_policja_samochod_przyczepka", Title = "LEGO Policja samoch�d z przyczepk�", Image = "~/images/lego_policja_samochod_przyczepka.jpg" },
                new HomeProductModel { Key = "lego_starwars_assaultonhoth", Title = "LEGO Star Wars Assault on Hoth", Image = "~/images/lego_starwars_assaultonhoth.jpg" },
                new HomeProductModel { Key = "lego_starwars_atap_walker", Title = "LEGO Star Wars AT-AP Walker", Image = "~/images/lego_starwars_atap_walker.jpg" },
                new HomeProductModel { Key = "lego_starwars_deathstar", Title = "LEGO Star Wars Death Star", Image = "~/images/lego_starwars_deathstar.jpg" },
                new HomeProductModel { Key = "lego_starwars_fo_snowspeeder", Title = "LEGO Star Wars FO Snowspeeder", Image = "~/images/lego_starwars_fo_snowspeeder.jpg" },
                new HomeProductModel { Key = "lego_starwars_fo_transporter", Title = "LEGO Star Wars FO Transporter", Image = "~/images/lego_starwars_fo_transporter.jpg" },
                new HomeProductModel { Key = "lego_starwars_jedi_defenderclasscruiser", Title = "LEGO Star Wars Jedi Defender-class Cruiser", Image = "~/images/lego_starwars_jedi_defenderclasscruiser.jpg" },
                new HomeProductModel { Key = "lego_starwars_milleniumfalcon", Title = "LEGO Star Wars Millennium Falcon", Image = "~/images/lego_starwars_milleniumfalcon.jpg" },
                new HomeProductModel { Key = "lego_starwars_republicdropship", Title = "LEGO Star Wars Republic Dropship", Image = "~/images/lego_starwars_republicdropship.jpg" },
                new HomeProductModel { Key = "lego_starwars_sandcrawler", Title = "LEGO Star Wars Sandcrawler", Image = "~/images/lego_starwars_sandcrawler.jpg" },


                // gry edukacyjne
                new HomeProductModel { Key = "gra_memory", Title = "Gra Memory", Image = "~/images/Gra-memory.jpg" },
                new HomeProductModel { Key = "puzzle_1000", Title = "Puzzle 1000 elementów", Image = "~/images/puzzle.jpg" },
            };

            // Losujemy 4 produkty jako nowości
            var rnd = new Random();
            ViewData["NewProducts"] = allProducts.OrderBy(x => rnd.Next()).Take(4).ToList();

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
