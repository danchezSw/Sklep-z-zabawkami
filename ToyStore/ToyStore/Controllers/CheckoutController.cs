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
                    var productIds = cartItems.Select(c => c.ProductId).ToList();
                    var productsInDb = _dbContext.Products
                                        .Where(p => productIds.Contains(p.Id))
                                        .ToDictionary(p => p.Id, p => p);

                    var orderItems = cartItems
                        .Where(c => productsInDb.ContainsKey(c.ProductId))
                        .Select(c => new OrderItem
                        {
                            OrderId = order.Id,
                            ProductId = c.ProductId,
                            Product = productsInDb[c.ProductId],
                            Quantity = c.Quantity,
                            UnitPrice = c.Price
                        })
                        .ToList();

                    _dbContext.OrderItems.AddRange(orderItems);
                    _dbContext.SaveChanges();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    ModelState.AddModelError("", "Wystąpił błąd przy zapisywaniu zamówienia.");
                    return View("Index", model);
                }
            }

            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Success");
        }


        public IActionResult Success()
        {
            return View();
        }
    }
}
