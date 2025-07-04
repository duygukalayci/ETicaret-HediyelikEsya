using GiftShop.Data;
using GiftShop.Entity.Entities;
using GiftShop.WebUI.Utils;
using GiftShop.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GiftShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class ProductsController : Controller
    {
        private readonly DatabaseContext _context;

        public ProductsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.Products.Include(p => p.Category);
            return View(await databaseContext.ToListAsync());
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            var model = new ProductViewModel
            {
                ParentCategories = _context.Categories
                   .Where(c => c.ParentCategoryID == null && c.IsActive)
                   .Select(c => new SelectListItem
                   {
                       Value = c.CategoryID.ToString(),
                       Text = c.Name
                   }).ToList(),

                SubCategories = new List<SelectListItem>() // boş başlat
            };

            return View(model);
        }


        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    model.Product.ImageUrl = await FileHelper.UploadFileAsync(Image, "/Img/");
                }

                _context.Products.Add(model.Product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Sayfa geri döndüğünde dropdown'lar boş olmasın diye tekrar doldur
            model.ParentCategories = _context.Categories
                .Where(c => c.ParentCategoryID == null)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryID.ToString(),
                    Text = c.Name
                }).ToList();

            if (model.SelectedParentCategoryID != null)
            {
                model.SubCategories = _context.Categories
                    .Where(c => c.ParentCategoryID == model.SelectedParentCategoryID)
                    .Select(c => new SelectListItem
                    {
                        Value = c.CategoryID.ToString(),
                        Text = c.Name
                    }).ToList();
            }

            return View(model);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .ThenInclude(c => c.ParentCategory)
                .FirstOrDefaultAsync(p => p.ProductID == id);

            if (product == null)
                return NotFound();

            // Üst kategoriler (ParentCategoryID == null)
            var parentCategories = await _context.Categories
                .Where(c => c.ParentCategoryID == null && c.IsActive)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryID.ToString(),
                    Text = c.Name
                }).ToListAsync();

            int? selectedParentId = null;

            // Seçili alt kategori
            int? selectedSubCategoryId = product.CategoryID;

            if (product.Category != null)
            {
                // Eğer seçili kategori alt kategori ise, üst kategori onun ParentCategoryID'si olur
                selectedParentId = product.Category.ParentCategoryID ?? product.Category.CategoryID;
            }

            // Alt kategorileri seçili üst kategoriye göre getir
            var subCategories = new List<SelectListItem>();
            if (selectedParentId != null)
            {
                subCategories = await _context.Categories
                    .Where(c => c.ParentCategoryID == selectedParentId && c.IsActive)
                    .Select(c => new SelectListItem
                    {
                        Value = c.CategoryID.ToString(),
                        Text = c.Name
                    }).ToListAsync();
            }

            ViewBag.ParentCategories = parentCategories;
            ViewBag.SubCategories = subCategories;
            ViewBag.SelectedParentCategoryID = selectedParentId;
            ViewBag.SelectedSubCategoryID = selectedSubCategoryId;

            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile? Image, bool cbResmiSil = false, int? selectedParentCategoryID = null)
        {
            Console.WriteLine($"selectedParentCategoryID: {selectedParentCategoryID}");
            Console.WriteLine($"product.CategoryID: {product.CategoryID}");
            if (id != product.ProductID)
                return NotFound();

            if (cbResmiSil && !string.IsNullOrEmpty(product.ImageUrl))
            {
                FileHelper.FileRemover(product.ImageUrl, "/Img/");
                product.ImageUrl = null;
            }

            if (Image != null)
            {
                product.ImageUrl = await FileHelper.UploadFileAsync(Image, "/Img/");
            }

            // Burada product.CategoryID zaten formdan gelen alt kategori seçimi olmalı
            // selectedParentCategoryID sadece dropdown seçimi için kullanılır, zorunlu değil

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(e => e.ProductID == id))
                        return NotFound();
                    else
                        throw;
                }
            }

            // Dropdownları yeniden yükle (hata durumunda)
            var parentCategoryId = selectedParentCategoryID;

            // Eğer üst kategori formdan gelmezse, product'ın kategorisinden bul
            if (parentCategoryId == null)
            {
                parentCategoryId = await _context.Categories
                    .Where(c => c.CategoryID == product.CategoryID)
                    .Select(c => c.ParentCategoryID ?? c.CategoryID)
                    .FirstOrDefaultAsync();
            }

            ViewBag.SelectedParentCategoryID = parentCategoryId;

            ViewBag.ParentCategories = await _context.Categories
                .Where(c => c.ParentCategoryID == null && c.IsActive)
                .Select(c => new SelectListItem { Value = c.CategoryID.ToString(), Text = c.Name })
                .ToListAsync();

            ViewBag.SubCategories = await _context.Categories
                .Where(c => c.ParentCategoryID == parentCategoryId && c.IsActive)
                .Select(c => new SelectListItem { Value = c.CategoryID.ToString(), Text = c.Name })
                .ToListAsync();

            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    FileHelper.FileRemover(product.ImageUrl);

                }
                _context.Products.Remove(product);
                if (product != null)
                {
                    _context.Products.Remove(product);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetSubCategories(int parentId)
        {
            var subCategories = _context.Categories
                .Where(c => c.ParentCategoryID == parentId && c.IsActive)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryID.ToString(),
                    Text = c.Name
                }).ToList();

            return Json(subCategories);
        }


        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}
