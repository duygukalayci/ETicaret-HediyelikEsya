using GiftShop.Entity.Entities;

namespace GiftShop.WebUI.Models
{
    public class CartViewModel
    {
        public List<CartLine>? CartLines { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
