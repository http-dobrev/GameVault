using Logic.Entities;
using Logic.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Security.Claims;
using UI.Models;

namespace UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserRegisterViewModel userRegisterViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                var request = new RegisterRequest
                {
                    Username = userRegisterViewModel.Username,
                    Email = userRegisterViewModel.Email,
                    Password = userRegisterViewModel.Password
                };

                _authService.Register(request);
                return RedirectToAction(nameof(Login));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(userRegisterViewModel);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var request = new LoginRequest
                {
                    EmailOrUsername = vm.EmailOrUsername,
                    Password = vm.Password
                };

                var user = _authService.Login(request);

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    };

                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = vm.RememberMe,
                    ExpiresUtc = vm.RememberMe
                        ? DateTimeOffset.UtcNow.AddDays(30)
                        : DateTimeOffset.UtcNow.AddHours(1)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    authProperties);

                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentException)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
