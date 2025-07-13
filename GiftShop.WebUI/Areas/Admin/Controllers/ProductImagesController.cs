using GiftShop.Data;
using GiftShop.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GiftShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductImagesController : Controller
    {
        private readonly DatabaseContext _context;

        public ProductImagesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductImages
        public async Task<IActionResult> Index(int? productId)
        {
            var databaseContext = _context.ProductImages.Include(p => p.Product);
            if (productId.HasValue)
            {
                return View(await databaseContext.Where(x=>x.ProductID==productId).ToListAsync());

            }
            return View(await databaseContext.ToListAsync());
        }

        // GET: Admin/ProductImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productImage = await _context.ProductImages
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductImageID == id);
            if (productImage == null)
            {
                return NotFound();
            }

            return View(productImage);
        }

        // GET: Admin/ProductImages/Create
        public IActionResult Create(int? productId)
        {
            ViewBag.ProductID = new SelectList(_context.Products, "ProductID", "Name", productId);
            return View();
        }

        // POST: Admin/ProductImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int productId, IFormFile imageFile, string alt)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                var newImage = new ProductImage
                {
                    ProductID = productId,
                    ImageUrl = fileName,
                    Alt = alt
                };

                _context.ProductImages.Add(newImage);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index"); // Bu ProductImages/Index sayfasına gider
            }

            // Hata varsa dropdown doldur
            ViewBag.ProductID = new SelectList(_context.Products, "ProductID", "Name", productId);
            return View();
        }

        // GET: Admin/ProductImages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var productImage = await _context.ProductImages.FindAsync(id);
            if (productImage == null)
                return NotFound();

            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", productImage.ProductID);

            return View(productImage);
        }

        // POST: Admin/ProductImages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductImageID,Name,Alt,ImageUrl,ProductID")] ProductImage productImage, IFormFile? imageFile)
        {
            if (id != productImage.ProductImageID)
                return NotFound();

            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Img", fileName);

                    using var stream = new FileStream(path, FileMode.Create);
                    await imageFile.CopyToAsync(stream);

                    productImage.ImageUrl = fileName;
                }

                try
                {
                    _context.Update(productImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ProductImages.Any(e => e.ProductImageID == productImage.ProductImageID))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", productImage.ProductID);
            return View(productImage);
        }


        // GET: Admin/ProductImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productImage = await _context.ProductImages
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductImageID == id);
            if (productImage == null)
            {
                return NotFound();
            }

            return View(productImage);
        }

        // POST: Admin/ProductImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productImage = await _context.ProductImages.FindAsync(id);
            if (productImage != null)
            {
                _context.ProductImages.Remove(productImage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductImageExists(int id)
        {
            return _context.ProductImages.Any(e => e.ProductImageID == id);
        }
    }
}
