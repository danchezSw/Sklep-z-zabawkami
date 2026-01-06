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

        private List<CartItemVm> MapCart(List<WebCartItem> cartItems)
        {
            return cartItems.Select(c => new CartItemVm
            {
                ProductId = c.ProductId,
                ProductName = c.ProductName,
                Image = c.Image,
                Quantity = c.Quantity,
                Price = c.Price,
                Color = c.Color
            }).ToList();
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
                model.CartItems = MapCart(cartItems);
                return View("Index", model);
            }

            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var order = new Order
                {
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    ShippingAddress = model.ShippingAddress,
                    PaymentMethod = model.PaymentMethod,
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
                return RedirectToAction("Start", "Payment", new { orderId = order.Id });
            }
            catch
            {
                transaction.Rollback();
                ModelState.AddModelError("", "Błąd zapisu zamówienia");
                model.CartItems = MapCart(cartItems);
                return View("Index", model);
            }
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
