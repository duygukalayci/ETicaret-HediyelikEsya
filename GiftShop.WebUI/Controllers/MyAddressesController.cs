using GiftShop.Business.Abstract;
using GiftShop.Entity.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;


namespace GiftShop.WebUI.Controllers
{
    [Authorize]
    public class MyAddressesController : Controller
    {
        private readonly IService<AppUser> _serviceAppUser;
        private readonly IService<Address> _serviceAddress;
        public MyAddressesController(IService<AppUser> serviceAppUser, IService<Address> serviceAddress)
        {
            _serviceAppUser = serviceAppUser;
            _serviceAddress = serviceAddress;
        }
        public async Task<IActionResult> Index()
        {
            // Kullanıcının AppUserID'sini claim'den al
            var userIdString = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                // Kullanıcı giriş yapmamışsa veya claim hatalıysa
                return RedirectToAction("SignIn", "Account");
            }

            // Kullanıcı adreslerini al
            var addresses = await _serviceAddress.GetAllAsync(x => x.AppUserID == userId);

            return View(addresses);

        }
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Address address)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("SignIn", "Account"); // Giriş yoksa buraya

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
                return RedirectToAction("SignIn", "Account");

            var appUser = await _serviceAppUser.GetAsync(x => x.AppUserID == userId);
            if (appUser == null)
                return RedirectToAction("SignIn", "Account");

            address.AppUserID = appUser.AppUserID;
            _serviceAddress.Add(address);
            await _serviceAddress.SaveChangesAsync();

            TempData["Message"] = "Adres başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task< IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var userIdString = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("SignIn", "Account");
            }

            var address = await _serviceAddress.GetAsync(a => a.Id == id && a.AppUserID == userId);
            if (address == null)
                return NotFound();

            return View(address);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Address address)
        {
            if (id != address.Id)
                return BadRequest();

            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("SignIn", "Account");

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
                return RedirectToAction("SignIn", "Account");

            if (ModelState.IsValid)
            {
                var existingAddress = await _serviceAddress.GetAsync(a => a.Id == id && a.AppUserID == userId);
                if (existingAddress == null)
                    return NotFound();

                // Güncellenecek alanları kopyala
                existingAddress.Title = address.Title;
                existingAddress.City = address.City;
                existingAddress.Disctrict = address.Disctrict;
                existingAddress.OpenAddress = address.OpenAddress;
                existingAddress.IsActive = address.IsActive;
                existingAddress.IsBillingAddress = address.IsBillingAddress;
                existingAddress.IsDeliveryAddress = address.IsDeliveryAddress;

                _serviceAddress.Update(existingAddress);
                await _serviceAddress.SaveChangesAsync();

                TempData["Message"] = "Adres başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(address);
        }



        // GET: MyAddresses/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var userIdString = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                return RedirectToAction("SignIn", "Account");

            var address = await _serviceAddress.GetAsync(x => x.Id == id && x.AppUserID == userId);

            if (address == null)
                return NotFound();

            return View(address);
        }

        // POST: MyAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userIdString = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                return RedirectToAction("SignIn", "Account");

            var address = await _serviceAddress.GetAsync(x => x.Id == id && x.AppUserID == userId);

            if (address == null)
                return NotFound();

            _serviceAddress.Delete(address);
            await _serviceAddress.SaveChangesAsync();

            TempData["Message"] = "Adres başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }


    }
}
