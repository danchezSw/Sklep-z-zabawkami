using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ToyStore.Data;
using ToyStore.Model.DataModels;
using ToyStore.ViewModels.VM;
using WebCartItem = ToyStore.Web.Models.CartItem;
using ToyStore.Web.Services;

namespace ToyStore.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly EmailService _emailService;
        public CheckoutController(ApplicationDbContext dbContext, EmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
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
        public async Task<IActionResult> PlaceOrder(CheckoutVm model)
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
                await _dbContext.SaveChangesAsync();

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
                await _dbContext.SaveChangesAsync();

                transaction.Commit();

                try
                {
                    decimal totalAmount = orderItems.Sum(x => x.Quantity * x.UnitPrice);
                    string subject = $"Potwierdzenie zamówienia nr #{order.Id} - ToyStore";

                    string body = $@"
                        <div style='font-family: Arial, sans-serif; padding: 20px; border: 1px solid #ddd; border-radius: 5px;'>
                            <h2 style='color: #0d6efd;'>Dziękujemy za zamówienie, {model.CustomerName}!</h2>
                            <p>Twój numer zamówienia: <strong>#{order.Id}</strong></p>
                            <p>Adres dostawy: {model.ShippingAddress}</p>
                            <hr />
                            <h3>Podsumowanie:</h3>
                            <ul>";

                    foreach (var item in cartItems)
                    {
                        string colorInfo = !string.IsNullOrEmpty(item.Color) ? $" (Kolor: {item.Color})" : "";
                        body += $"<li>{item.ProductName}{colorInfo} - {item.Quantity} szt. x {item.Price:C}</li>";
                    }

                    body += $@"
                            </ul>
                            <h3>Do zapłaty: {totalAmount:C}</h3>
                            <p>Metoda płatności: {model.PaymentMethod}</p>
                            <br/>
                            <p>Pozdrawiamy,<br/>Zespół ToyStore</p>
                        </div>";

                    if (!string.IsNullOrEmpty(model.CustomerEmail))
                    {
                        await _emailService.SendEmailAsync(model.CustomerEmail, subject, body);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd wysyłania e-maila: {ex.Message}");
                }

                HttpContext.Session.Remove("Cart");

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
