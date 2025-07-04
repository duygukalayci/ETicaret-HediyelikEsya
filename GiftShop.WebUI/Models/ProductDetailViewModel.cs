using GiftShop.Entity.Entities;

namespace GiftShop.WebUI.Models
{
    public class ProductDetailViewModel
    {
        public Product? Product { get; set; }
        public IEnumerable<Product>? RelatedProducts { get; set; }
        public bool IsFavorite { get; set; }  // Yeni 

    }
}
