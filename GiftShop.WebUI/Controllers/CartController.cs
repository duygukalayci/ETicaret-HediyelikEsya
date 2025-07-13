using GiftShop.Business.Abstract;
using GiftShop.Business.Concrete;
using GiftShop.Data;
using GiftShop.Entity.Entities;
using GiftShop.WebUI.ExtensionMethods;
using GiftShop.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftShop.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IService<Product> _productService;
        private readonly IService<Address> _serviceAddress;
        private readonly IService<AppUser> _serviceAppUser;
        private readonly IService<Order> _serviceOrder;
        private readonly DatabaseContext _context;

        public CartController(IService<Product> productService, IService<Address> serviceAddress, IService<AppUser> serviceAppUser, IService<Order> serviceOrder, DatabaseContext context)
        {
            _productService = productService;
            _serviceAddress = serviceAddress;
            _serviceAppUser = serviceAppUser;
            _serviceOrder = serviceOrder;
            _context = context;
        }
        public IActionResult Index()
        {
            var cart = GetCart();
            var model = new CartViewModel
            {
                CartLines = cart.CartLines,
                TotalPrice = cart.TotalPrice()
            };

            return View(model);
        }
        public IActionResult Add(int ProductID, int Quantity = 1)
        {
            var product = _productService.Find(ProductID);
            if (product != null)
            {
                var cart = GetCart();
                cart.AddProduct(product, Quantity);
                HttpContext.Session.SetJson("Cart", cart);
                return Redirect(Request.Headers["Referer"].ToString());
            }
            return RedirectToAction("Index");
        }
        public IActionResult Update(int ProductID, int Quantity = 1)
        {
            var product = _productService.Find(ProductID);
            if (product != null)
            {
                var cart = GetCart();
                cart.UpdateProduct(product, Quantity);
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Remove(int ProductID)
        {
            var product = _productService.Find(ProductID);
            if (product != null)
            {
                var cart = GetCart();
                cart.RemoveProduct(product);
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<IActionResult> CheckOut()
        {
            var cart = GetCart();
            var appUser = await _serviceAppUser.GetAsync(x => x.UserName == User.Identity.Name);

            if (appUser == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            var addresses = await _serviceAddress.GetAllAsync(x => x.AppUserID == appUser.AppUserID && x.IsActive);
            var model = new CheckOutViewModel
            {
                CartProducts = cart.CartLines,
                TotalPrice = cart.TotalPrice(),
                Addresses = addresses,
            };

            return View(model);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(
         int deliveryAddress,
         int billingAddress,
         string CardName,
         string CardNumber,
         string CardMonth,
         string CardYear,
         string CVV)
        {



            var cart = GetCart();
            var appUser = await _serviceAppUser.GetAsync(x => x.UserName == User.Identity.Name);

            if (appUser == null)
                return RedirectToAction("SignIn", "Account");

            var addresses = await _serviceAddress.GetAllAsync(x => x.AppUserID == appUser.AppUserID && x.IsActive);

            // Kart bilgileri kontrolü
            if (string.IsNullOrWhiteSpace(CardNumber) || CardNumber.Length < 12 ||
                string.IsNullOrWhiteSpace(CardMonth) || string.IsNullOrWhiteSpace(CardYear) ||
                string.IsNullOrWhiteSpace(CVV))
            {
                ModelState.AddModelError("", "Lütfen kart bilgilerini eksiksiz giriniz.");
            }

            // Adres kontrolü
            var deliveryAddressEntity = addresses.FirstOrDefault(a => a.Id == deliveryAddress);
            var billingAddressEntity = addresses.FirstOrDefault(a => a.Id == billingAddress);

            if (deliveryAddressEntity == null)
                ModelState.AddModelError("", "Geçersiz teslimat adresi.");
            if (billingAddressEntity == null)
                ModelState.AddModelError("", "Geçersiz fatura adresi.");

            // Hatalıysa tekrar formu göster
            if (!ModelState.IsValid)
            {
                var model = new CheckOutViewModel
                {
                    CartProducts = cart.CartLines,
                    TotalPrice = cart.TotalPrice(),
                    Addresses = addresses
                };
                return View(model);
            }

            var order = new Order
            {
                AppUserID = appUser.AppUserID,
                OrderDate = DateTime.Now,
                TotalAmount = cart.TotalPrice(),
                DeliveryAddress = $"{deliveryAddressEntity.Title} - {deliveryAddressEntity.City} / {deliveryAddressEntity.Disctrict} - {deliveryAddressEntity.OpenAddress}",
                BillingAddress = $"{billingAddressEntity.Title} - {billingAddressEntity.City} / {billingAddressEntity.Disctrict} - {billingAddressEntity.OpenAddress}",
                Status = "Yeni",
                OrderDetails = cart.CartLines.Select(item => new OrderDetail
                {
                    ProductID = item.Product.ProductID,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                }).ToList()
            };


            try
            {
                await _serviceOrder.AddAsync(order);
                await _serviceOrder.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Sipariş kaydı sırasında bir hata oluştu: " + ex.Message);
                var model = new CheckOutViewModel
                {
                    CartProducts = cart.CartLines,
                    TotalPrice = cart.TotalPrice(),
                    Addresses = addresses
                };
                return View(model);
            }

            HttpContext.Session.Remove("Cart");
            TempData["Message"] = "Siparişiniz başarıyla alındı.";
            return RedirectToAction("CompleteOrder");
        }


        [Authorize]
        public IActionResult CompleteOrder()
        {
            ViewBag.Message = TempData["Message"] ?? "Siparişiniz başarıyla tamamlandı.";
            return View(); // CompleteOrder.cshtml çağrılır
        }
        private CartService GetCart()
        {
            return HttpContext.Session.GetJson<CartService>("Cart") ?? new CartService();

        }
    }
}
