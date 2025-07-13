using GiftShop.Business.Abstract;
using GiftShop.Data;
using GiftShop.Entity.Entities;
using GiftShop.WebUI.Models;
using GiftShop.WebUI.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;

namespace GiftShop.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IService<AppUser> _service;
        private readonly IService<Order> _serviceOrder;
        private readonly DatabaseContext _context;

        public AccountController(IService<AppUser> service, IService<Order> serviceOrder, DatabaseContext context)
        {
            _service = service;
            _serviceOrder = serviceOrder;
            _context = context;
        }

        // Giriş sayfası
        [HttpGet]
        public IActionResult SignIn()
        {

            return View();
        }
        [Authorize]
        public async Task<IActionResult> MyOrders()
        {
            var userName = HttpContext.User.FindFirst("UserName")?.Value;
            if (string.IsNullOrEmpty(userName))
                return Unauthorized();

            var user = await _service.GetAsync(x => x.UserName == userName);
            if (user == null)
                return NotFound();

            // Siparişleri çekiyoruz (OrderDetails ve Product bilgileri dahil)
            var orders = await _context.Orders
                .Where(o => o.AppUserID == user.AppUserID)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ToListAsync();
               // Ürün bilgileri de dahil

            return View("MyOrders", orders.OrderByDescending(o => o.OrderDate).ToList());
        }

        // Giriş işlemi
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginViewModel model, string returnUrl = null)
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
                new Claim(ClaimTypes.NameIdentifier, user.AppUserID.ToString()), // ← BU EKLENECEK
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserName", user.UserName),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "Customer")
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            if (user.IsAdmin)
            {
                return RedirectToAction("Index", "Main", new { area = "Admin" });
            }


            return RedirectToAction("Index", "Home");
        }



        // Profil görüntüleme
        [HttpGet]
        [Authorize]

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
            return RedirectToAction(nameof(Profile));
        }

        // Kayıt sayfası
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        // Kayıt işlemi
        [HttpPost]
        public async Task<IActionResult> SignUp(AppUser appUser)
        {
            appUser.IsAdmin = false;

            if (!ModelState.IsValid)
                return View("SignUp", appUser);
            await _service.AddAsync(appUser);
            await _service.SaveChangesAsync();

            TempData["Success"] = "🎉 Kayıt başarılı! Giriş yapabilirsiniz.";

            return RedirectToAction("SignIn");
        }

        // Çıkış
        public async Task<IActionResult> SignOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult PasswordRenew()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordRenew(string email, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(newPassword))
            {
                TempData["Error"] = "Email ve yeni şifre boş olamaz.";
                return RedirectToAction("PasswordRenew");
            }

            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                TempData["Error"] = "Bu email ile kayıtlı kullanıcı bulunamadı.";
                return RedirectToAction("PasswordRenew");
            }

            user.Password = newPassword; // 🔒 Şifre hashlemeyi sonra ekleyelim
            _context.Update(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "✔️ Şifreniz başarıyla güncellendi. Giriş yapabilirsiniz.";
            return RedirectToAction("PasswordRenew");
        }


        [HttpGet]
        public async Task<IActionResult> PasswordChangeAsync(string user)
        {
            if (string.IsNullOrEmpty(user))
            {
                return BadRequest("Geçersiz istek");
            }

            var appUser = await _context.AppUsers.FirstOrDefaultAsync(u => u.Email == user);
            if (appUser == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("PasswordRenew"); // Geri yönlendirme sayfası
            }

            ViewBag.Email = user;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordChange(string email, string newPassword)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(newPassword))
            {
                ModelState.AddModelError("", "Tüm alanları doldurmalısınız.");
                return View();
            }

            var appUser = await _context.AppUsers.FirstOrDefaultAsync(u => u.Email == email);
            if (appUser == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                return View();
            }

            appUser.Password = newPassword; // ❗ Not: Şifre hash'lenmeli!
            _context.AppUsers.Update(appUser);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Şifreniz başarıyla güncellendi.";
            return RedirectToAction("SignIn");
        }


    }
}

