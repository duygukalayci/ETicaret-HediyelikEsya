using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop.Entity.Entities
{
    public class Shipment
    {
        [Display(Name = "Kargo No")]
        public int ShipmentID { get; set; }
        public int OrderID { get; set; }
        [Display(Name = "Sipariş No")]
        public Order Order { get; set; }

        [Display(Name = "Kargo Firması")]
        public string Carrier { get; set; }  // Kargo Firması
        [Display(Name = "Takip Numarası")]
        public string TrackingNumber { get; set; }  // Takip Numarası


        [Display(Name = "Gönderim Tarihi")]
        public DateTime? ShippedDate { get; set; }  // Gönderim Tarihi
        [Display(Name = "Teslim Tarihi")]
        public DateTime? DeliveredDate { get; set; }  // Teslim Tarihi

    }
}
