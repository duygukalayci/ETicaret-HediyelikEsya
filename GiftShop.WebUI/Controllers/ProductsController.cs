using GiftShop.Business.Abstract;
using GiftShop.Entity.Entities;
using GiftShop.WebUI.ExtensionMethods;
using GiftShop.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;   

namespace GiftShop.WebUI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IService<Product> _productService;
        private readonly IService<Category> _categoryService;

        public ProductsController(IService<Product> productService, IService<Category> categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult Index(int? categoryId)
        {
            List<Product> products;

            if (categoryId == null)
            {
                products = _productService.GetAll();
                ViewBag.CategoryName = "Tüm Ürünler";
            }
            else
            {
                var allCategories = _categoryService.GetAll();

                var categoryIds = allCategories
                    .Where(c => c.CategoryID == categoryId || c.ParentCategoryID == categoryId)
                    .Select(c => c.CategoryID)
                    .ToList();

                products = _productService.GetAll(p => categoryIds.Contains(p.CategoryID));

                var category = _categoryService.Find(categoryId.Value);
                ViewBag.CategoryName = category?.Name ?? "Kategori";
            }

            return View(products);
        }

        public IActionResult ByCategory(int categoryId)
        {
            var allCategories = _categoryService.GetAll();

            // Seçilen kategori + alt kategorilerin ID’leri
            var categoryIds = allCategories
                .Where(c => c.CategoryID == categoryId || c.ParentCategoryID == categoryId)
                .Select(c => c.CategoryID)
                .ToList();

            // Ürünleri bu kategori id’lerine göre filtrele
            var products = _productService.GetAll(p => categoryIds.Contains(p.CategoryID));

            var category = _categoryService.Find(categoryId);
            ViewBag.CategoryName = category?.Name ?? "Kategori";

            return View("Index", products);
        }

        public IActionResult Details(int id)
        {
            if (id == 0)
                return NotFound();

            var product = _productService.GetAllQueryable()
                     .Where(p => p.ProductID == id && p.IsActive)
                     .Include(p => p.Category)
                     .Include(p => p.ProductImages)  
                     .FirstOrDefault();

            if (product == null)
                return NotFound();

            var favorites = HttpContext.Session.GetJson<List<Product>>("GetFavorites") ?? new List<Product>();

            var model = new ProductDetailViewModel
            {
                Product = product,
                RelatedProducts = _productService.GetAll(p =>
                        p.IsActive && p.CategoryID == product.CategoryID && p.ProductID != product.ProductID)
                    .Take(4).ToList(),
                IsFavorite = favorites.Any(f => f.ProductID == id)
            };

            return View(model);
        }


        public IActionResult Search(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return RedirectToAction("Index");
            }

            var results = _productService.GetAll(p => p.Name.Contains(q));
            return View(results);
        }
    }
}
