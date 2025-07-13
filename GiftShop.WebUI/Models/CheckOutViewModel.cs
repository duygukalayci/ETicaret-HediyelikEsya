using GiftShop.Entity.Entities;

namespace GiftShop.WebUI.Models
{
    public class CheckOutViewModel
    {
        public List<CartLine>? CartProducts { get; set; }
        public decimal TotalPrice { get; set; }
        public List<Address>? Addresses { get; set; }

    }
}
