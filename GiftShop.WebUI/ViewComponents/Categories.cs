using GiftShop.Business.Abstract;
using GiftShop.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftShop.WebUI.ViewComponents
{
    public class Categories : ViewComponent
    {
        private readonly IService<Category> _categoryService;

        public Categories(IService<Category> categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService
                .GetAllQueryable()
                .Include(c => c.SubCategories)
                .ToListAsync();

            return View(categories);
        }

    }
}
