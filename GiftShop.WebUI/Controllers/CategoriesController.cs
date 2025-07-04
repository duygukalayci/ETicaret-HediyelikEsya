using GiftShop.Business.Abstract;
using GiftShop.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GiftShop.WebUI.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IService<Category> _service;

        public CategoriesController(IService<Category> service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                // Tüm üst kategorileri (ParentCategoryID == null) ve alt kategorileri getir
                var categories = await _service
                    .GetAllQueryable()
                    .Where(c => c.ParentCategoryID == null)
                    .Include(c => c.SubCategories)
                    .ToListAsync();

                return View(categories);
            }
            else
            {
                // Belirli kategoriye ait ürünleri ve alt kategorileri getir
                var category = await _service
                    .GetAllQueryable()
                    .Include(c => c.Products)
                    .Include(c => c.SubCategories)
                    .FirstOrDefaultAsync(c => c.CategoryID == id);

                if (category == null)
                    return NotFound();

                return View("CategoryDetails", category);
            }
        }
    }
}
