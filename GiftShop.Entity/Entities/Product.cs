using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop.Entity.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        [Display(Name = "Adı")]
        public string Name { get; set; }
        [Display(Name = "Açıklama")]

        public string? Description { get; set; }
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }
        [Display(Name = "Resim")]
        public string? ImageUrl { get; set; }
        [Display(Name = "Stok Miktarı")]
        public int Stock { get; set; }
        [Display(Name = "Kayıt Tarihi"), ScaffoldColumn(false)]
        public DateTime? CreateDate { get; set; }= DateTime.Now;
        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; }


        // Kategori ilişkisi

        [Required(ErrorMessage = "Kategori seçilmesi zorunludur")]
        [Display(Name = "Kategori")]
        public int CategoryID { get; set; }
        public Category? Category { get; set; }

        // Sipariş ilişkisi
        public List<OrderDetail> OrderDetails { get; set; }= new List<OrderDetail>();

        public List<ProductImage>? ProductImages { get; set; } = new List<ProductImage>();


    }
}
