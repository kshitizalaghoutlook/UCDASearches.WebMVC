using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UCDASearches.WebMVC.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            // TODO: replace with real auth (Identity or your DB)
            if (model.Email == "test@test.com" && model.Password == "123456")
            {
                // return LocalRedirect(returnUrl ?? Url.Action("Index","Home")!);
                return RedirectToAction("Index", "Search");
            }

            ModelState.AddModelError(string.Empty, "Invalid credentials");
            return View(model);
        }
    }

    public class LoginViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = "";
        public bool RememberMe { get; set; }
    }
}
