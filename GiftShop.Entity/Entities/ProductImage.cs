using System.ComponentModel.DataAnnotations;

namespace GiftShop.Entity.Entities
{
    public class ProductImage
    {
        public int ProductImageID { get; set; }
        [Display(Name = " Resim Adı"),StringLength(240)]
        public string? Name { get; set; }
        [Display(Name = "Resim Açıklaması"), StringLength(240)]
        public string? Alt { get; set; }
        [Display(Name = "Resim ")]
        public string? ImageUrl { get; set; }

        public int? ProductID { get; set; }
        [Display(Name = "Ürün")]
        public Product? Product { get; set; }
    }
}
