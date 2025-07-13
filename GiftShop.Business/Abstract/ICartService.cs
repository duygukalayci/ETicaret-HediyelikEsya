using GiftShop.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop.Business.Abstract
{
    public interface ICartService
    {
        void AddProduct(Product product, int quantity);
        void RemoveProduct(Product product);
        void UpdateProduct(Product product, int quantity);
        decimal TotalPrice();
        void ClearAll();
    }
}
