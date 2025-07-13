using GiftShop.Business.Abstract;
using GiftShop.Entity.Entities;

namespace GiftShop.Business.Concrete
{
    public class CartService : ICartService
    {
        public List<CartLine> CartLines = new();
        public void AddProduct(Product product, int quantity)
        {
            var urun= CartLines.FirstOrDefault(x =>x.Product.ProductID==product.ProductID);
            if (urun != null)
            {
                urun.Quantity += quantity;
            }
            else
            {
                CartLines.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }

        }

        public void ClearAll()
        {
            CartLines.Clear();
        }

        public void RemoveProduct(Product product)
        {
            CartLines.RemoveAll(x => x.Product.ProductID == product.ProductID);
        }

        public decimal TotalPrice()
        {
            return CartLines.Sum(x => x.Product.Price * x.Quantity);
        }

        public void UpdateProduct(Product product, int quantity)
        {
            var urun = CartLines.FirstOrDefault(x => x.Product.ProductID == product.ProductID);
            if (urun != null)
            {
                urun.Quantity = quantity;
            }
            else
            {
                CartLines.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
        }
    }
}
