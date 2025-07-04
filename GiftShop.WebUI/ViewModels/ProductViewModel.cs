using GiftShop.Entity.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GiftShop.WebUI.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }=new Product();

        public int? SelectedParentCategoryID { get; set; }

        public List<SelectListItem> ParentCategories { get; set; } = new();
        public List<SelectListItem> SubCategories { get; set; } = new();
        public IFormFile? ImageFile { get; set; }

    }
}
