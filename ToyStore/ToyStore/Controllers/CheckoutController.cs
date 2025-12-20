using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ToyStore.Data;
using ToyStore.Model.DataModels;
using ToyStore.ViewModels.VM;
using WebCartItem = ToyStore.Web.Models.CartItem;

namespace ToyStore.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CheckoutController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var cartItems = HttpContext.Session.GetObjectFromJson<List<WebCartItem>>("Cart");
            if (cartItems == null || !cartItems.Any())
                return RedirectToAction("Index", "Cart");

            var model = new CheckoutVm
            {
                CartItems = cartItems.Select(c => new CartItemVm
                {
                    ProductId = c.ProductId,
                    ProductName = c.ProductName,
                    Image = c.Image,
                    Quantity = c.Quantity,
                    Price = c.Price,
                    Color = c.Color
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PlaceOrder(CheckoutVm model)
        {
            var cartItems = HttpContext.Session.GetObjectFromJson<List<WebCartItem>>("Cart");
            if (cartItems == null || !cartItems.Any())
                return RedirectToAction("Index", "Cart");

            if (!ModelState.IsValid)
            {
                model.CartItems = cartItems.Select(c => new CartItemVm
                {
                    ProductId = c.ProductId,
                    ProductName = c.ProductName,
                    Image = c.Image,
                    Quantity = c.Quantity,
                    Price = c.Price,
                    Color = c.Color
                }).ToList();

                return View("Index", model);
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in cartItems)
                    {
                        var productExists = _dbContext.Products.Any(p => p.Id == item.ProductId);
                        if (!productExists)
                        {
                            throw new Exception($"Produkt w koszyku o Id {item.ProductId} nie istnieje w bazie!");
                        }
                    }

                    var order = new Order
                    {
                        CustomerName = model.CustomerName,
                        CustomerEmail = model.CustomerEmail,
                        ShippingAddress = model.ShippingAddress,
                        Status = OrderStatus.Pending,
                        OrderDate = DateTime.Now
                    };

                    _dbContext.Orders.Add(order);
                    _dbContext.SaveChanges();

                    var orderItems = cartItems.Select(item => new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        ImageUrl = item.Image,
                        Color = item.Color,
                        Quantity = item.Quantity,
                        UnitPrice = item.Price
                    }).ToList();

                    _dbContext.OrderItems.AddRange(orderItems);
                    _dbContext.SaveChanges();

                    transaction.Commit();

                    HttpContext.Session.Remove("Cart");

                    return RedirectToAction("Success");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    foreach (var item in cartItems)
                    {
                        Console.WriteLine($"Produkt w koszyku: Id={item.ProductId}, Name={item.ProductName}");
                    }

                    ModelState.AddModelError("", "Wystąpił błąd przy zapisywaniu zamówienia: " + ex.Message);

                    model.CartItems = cartItems.Select(c => new CartItemVm
                    {
                        ProductId = c.ProductId,
                        ProductName = c.ProductName,
                        Image = c.Image,
                        Quantity = c.Quantity,
                        Price = c.Price,
                        Color = c.Color
                    }).ToList();

                    return View("Index", model);
                }
            }
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
