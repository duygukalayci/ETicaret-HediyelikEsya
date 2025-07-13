using GiftShop.Business.Abstract;
using GiftShop.Entity.Entities;
using GiftShop.WebUI.ExtensionMethods;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace GiftShop.WebUI.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IService<Product> _productService;

        public FavoritesController(IService<Product> productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            var favorites = GetFavorites();
            return View(favorites);
        }

        private List<Product> GetFavorites()
        {
            return HttpContext.Session.GetJson<List<Product>>("GetFavorites") ?? new List<Product>();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(int productId)
        {
            var favorites = GetFavorites();
            var product = _productService.Find(productId);

            if (product != null && !favorites.Any(p => p.ProductID == productId))
            {
                favorites.Add(product);
                HttpContext.Session.SetJson("GetFavorites", favorites);
                return Redirect(Request.Headers["Referer"].ToString());
            }

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int productId)
        {
            var favorites = GetFavorites();

            if (favorites.Any(p => p.ProductID == productId))
            {
                favorites.RemoveAll(p => p.ProductID == productId);
                HttpContext.Session.SetJson("GetFavorites", favorites);
            }

            return RedirectToAction("Index");
        }
    }
}
