using GiftShop.Business.Abstract;
using GiftShop.Entity.Entities;
using GiftShop.WebUI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GiftShop.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IService<AppUser> _service;

        public AccountController(IService<AppUser> service)
        {
            _service = service;
        }

        // Giriş sayfası
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        // Giriş işlemi
        [HttpPost]
        public async Task<IActionResult> SignInAsync(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _service.Get(u => u.Email == model.Email && u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Email veya şifre yanlış!");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.AppUserID.ToString()),
                new Claim("UserName", user.UserName),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "Customer")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (user.IsAdmin)
            {
                return RedirectToAction("Index", "Main", new { area = "Admin" });
            }

            return RedirectToAction("Index", "Home");
        }

        // Profil görüntüleme
        [HttpGet]
        public IActionResult Profile()
        {
            var userName = HttpContext.User.FindFirst("UserName")?.Value;
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            var user = _service.Get(u => u.UserName == userName);
            if (user == null)
                return NotFound();

            var model = new UserEditViewModel
            {
                AppUserID = user.AppUserID,
                UserName = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Phone = user.Phone,
                Password = "" // Şifre boş
            };

            return View(model);
        }

        // Profil güncelleme
        [HttpPost]
        public async Task<IActionResult> Profile(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _service.FindAsync(model.AppUserID);
            if (user == null)
                return NotFound();

            user.UserName = model.UserName;
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Email = model.Email;
            user.Phone = model.Phone;

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                user.Password = model.Password;
            }

            _service.Update(user);
            await _service.SaveChangesAsync();

            TempData["Message"] = "Profiliniz başarıyla güncellendi.";
            return RedirectToAction("Profile");
        }

        // Kayıt sayfası
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        // Kayıt işlemi
        [HttpPost]
        public async Task<IActionResult> SignUpAsync(AppUser appUser)
        {
            appUser.IsAdmin = false;

            if (!ModelState.IsValid)
                return View(appUser);

            await _service.AddAsync(appUser);
            await _service.SaveChangesAsync();

            return RedirectToAction("SignIn");
        }

        // Çıkış
        public async Task<IActionResult> SignOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("SignIn");
        }
    }
}
