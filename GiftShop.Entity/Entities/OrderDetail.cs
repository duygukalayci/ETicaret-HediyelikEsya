using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop.Entity.Entities
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        [Display(Name = "Sipariş Adedi")]
        public int Quantity { get; set; }
        [Display(Name = "Birim Fiyatı")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Toplam Tutar")]
        public decimal? TotalAmount { get; set; }



        // Sipariş ilişkisi
        public int OrderID { get; set; }
        [Display(Name = "Sipariş Bilgisi")]
        public Order Order { get; set; }

        // Ürün ilişkisi
        public int ProductID { get; set; }
        [Display(Name = "Ürün Bilgisi")]
        public Product Product { get; set; }
    }
}
