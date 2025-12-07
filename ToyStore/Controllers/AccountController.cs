using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToyStore.Model.DataModels;
using ToyStore.ViewModels.VM;

[Authorize]
public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IActionResult> ChangeAddress()
    {
        var user = await _userManager.GetUserAsync(User);
        var model = new ChangeAddressVm { Address = user.Address };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ChangeAddress(ChangeAddressVm model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.GetUserAsync(User);
        user.Address = model.Address;
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Adres został zaktualizowany!";
            return RedirectToAction("ChangeAddress");
        }

        foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
        return View(model);
    }

    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordVm model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            ModelState.AddModelError("", "Nie znaleziono użytkownika.");
            return View(model);
        }

        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            TempData["PasswordChangeError"] = "Nie udało się zmienić hasła.";
            return View(model);
        }

        await _signInManager.RefreshSignInAsync(user);
        TempData["PasswordChangeSuccess"] = "Hasło zostało zmienione pomyślnie.";
        return RedirectToAction("ChangePassword");
    }

}
