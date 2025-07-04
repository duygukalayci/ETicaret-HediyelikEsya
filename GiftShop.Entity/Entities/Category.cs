using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop.Entity.Entities
{
    public class Category
    {
        public int CategoryID { get; set; }
        [Display(Name = "Adı")]
        public string Name { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; } 
        [Display(Name = "Kayıt Tarihi"), ScaffoldColumn(false)]
        public DateTime? CreateDate { get; set; }
        [Display(Name = "Üst Menü")]
        public bool IsTopMenu { get; set; }  
        [Display(Name = "Üst Kategori")]
        public int? ParentCategoryID { get; set; } // null ise üst kategoridir


        public Category? ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();

        // Ürün ilişkisi

        // İlişki: Bir kategori birden fazla ürün içerebilir
        public List<Product> Products { get; set; } = new List<Product>();

    }
}
