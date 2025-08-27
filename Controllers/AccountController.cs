using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
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
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            // TODO: replace with real auth (Identity or your DB)
            if (model.Email == "test@test.com" && model.Password == "123456")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Email)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal);

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
