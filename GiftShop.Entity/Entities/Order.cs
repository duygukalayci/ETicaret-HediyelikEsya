using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop.Entity.Entities
{
    public class Order
    {
        [Display(Name = "Sipariş No")]
        public int OrderID { get; set; }
        [Display(Name = "Sipariş Tarihi")]
        public DateTime? OrderDate { get; set; }= DateTime.Now;
        [Display(Name = "Toplam Tutar")]
        public decimal? TotalAmount { get; set; }

        [Display(Name = "Toplam Tutar")]
        public decimal CalculatedTotalAmount => OrderDetails?.Sum(od => od.UnitPrice * od.Quantity) ?? 0;
        [Display(Name = "Sipariş Durumu")]
        public string Status { get; set; }

        /// // Kullanıcı ilişkisi
        /// 
        public int AppUserID { get; set; }
        [Display(Name = "Kullanıcı Bilgisi")]
        public AppUser AppUser { get; set; }




        public List<OrderDetail> OrderDetails{ get; set; }= new List<OrderDetail>();

        public Shipment Shipment { get; set; }  // Kargo bilgisi



    }
}
