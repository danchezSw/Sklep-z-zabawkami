using Microsoft.AspNetCore.Mvc;
using ToyStore.Data;
using ToyStore.Model.DataModels;
using ToyStore.ViewModels.VM;

public class PaymentController : Controller
{
    private readonly ApplicationDbContext _db;

    public PaymentController(ApplicationDbContext db)
    {
        _db = db;
    }

    private void FinalizePayment(int orderId)
    {
        var order = _db.Orders.FirstOrDefault(o => o.Id == orderId);
        if (order == null)
            throw new Exception("Zamówienie nie istnieje.");
        order.Status = OrderStatus.Paid;
        _db.SaveChanges();
        HttpContext.Session.Remove("Cart");
    }


    public IActionResult Start(int orderId)
    {
        var order = _db.Orders.FirstOrDefault(o => o.Id == orderId);
        if (order == null) return NotFound();

        return order.PaymentMethod switch
        {
            "Card" => RedirectToAction("Card", new { orderId }),
            "Blik" => RedirectToAction("Blik", new { orderId }),
            "Transfer" => RedirectToAction("Transfer", new { orderId }),
            _ => NotFound()
        };
    }

    public IActionResult Card(int orderId)
    {
        return View(new CardPaymentVm { OrderId = orderId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Card(CardPaymentVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        FinalizePayment(model.OrderId);
        return RedirectToAction("Success", "Checkout");
    }

    public IActionResult Blik(int orderId)
    {
        return View(new BlikPaymentVm { OrderId = orderId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Blik(BlikPaymentVm model)
    {
        if (!ModelState.IsValid)
            return View(model);
        FinalizePayment(model.OrderId);

        return RedirectToAction("Success", "Checkout");
    }

    public IActionResult Transfer(int orderId)
    {
        return View(new TransferPaymentVm { OrderId = orderId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Transfer(TransferPaymentVm model)
    {
        if (!ModelState.IsValid || !model.Confirmed)
        {
            ModelState.AddModelError("", "Musisz potwierdzić wykonanie przelewu.");
            return View(model);
        }

        FinalizePayment(model.OrderId);
        return RedirectToAction("Success", "Checkout");
    }

}
