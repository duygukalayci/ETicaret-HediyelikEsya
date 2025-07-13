using GiftShop.Business.Abstract;
using GiftShop.Entity.Entities;
using GiftShop.WebUI.Models;
using GiftShop.WebUI.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GiftShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IService<Slider> _sliderService;
        private readonly IService<Product> _productService;
        private readonly IService<ContactMessage> _contactService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IService<Slider> sliderService,
            IService<Product> productService,
            IService<ContactMessage> contactService,
            ILogger<HomeController> logger)
        {
            _sliderService = sliderService;
            _productService = productService;
            _contactService = contactService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomePageViewModel
            {
                Sliders = await _sliderService.GetAllAsync(),
                Products = await _productService.GetAllAsync(p => p.IsActive) // Sadece aktif ürünleri getir
            };

            return View(model);
        }

        public IActionResult Privacy() => View();

        [Route("AccessDenied")]
        public IActionResult AccessDenied() => View();

        public IActionResult About() => View();

        public IActionResult VizyonMisyon() => View();

        [HttpGet]
        public IActionResult ContactUs() => View();

        [HttpPost]
        public async Task<IActionResult> ContactUs(ContactMessage contactMessage)
        {
            if (!ModelState.IsValid)
                return View(contactMessage);

            try
            {
                await _contactService.AddAsync(contactMessage);
                await _contactService.SaveChangesAsync();

                TempData["Message"] = "Mesajýnýz baþarýyla gönderildi. En kýsa sürede sizinle iletiþime geçeceðiz.";
                // await MailHelper.SendMailAsync(contactMessage);
                return RedirectToAction("ContactUs");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Mesajýnýz alýnamadý. Lütfen daha sonra tekrar deneyin.");
                return View(contactMessage);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Blog()
        {
            return View();  
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
