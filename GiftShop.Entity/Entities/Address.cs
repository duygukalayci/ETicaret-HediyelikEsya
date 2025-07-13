using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop.Entity.Entities
{
    public class Address
    {
        public int Id { get; set; }
        [Display(Name = "Adres Başlığı"),StringLength(50), Required(ErrorMessage = "{0} Alanı Zorunludur!")]
        public string Title { get; set; }
        [Display(Name = "Şehir"), StringLength(50), Required(ErrorMessage = "{0} Alanı Zorunludur!")]
        public string City { get; set; }
        [Display(Name = "İlçe"), StringLength(50), Required(ErrorMessage = "{0} Alanı Zorunludur!")]
        public string Disctrict { get; set; }
        [Display(Name = "Açık Adres"), DataType(DataType.MultilineText),Required(ErrorMessage = "{0} Alanı Zorunludur!")]
        public string OpenAddress { get; set; }
        [Display(Name = "Aktif")]
        public bool IsActive { get; set; }
        [Display(Name = "Fatura Adresi")]
        public bool IsBillingAddress { get; set; }
        [Display(Name = "Teslimat Adresi")]
        public bool IsDeliveryAddress { get; set; }
        [Display(Name = "Oluşturulma Tarihi"),ScaffoldColumn(false)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [ScaffoldColumn(false)]
        public Guid? AddressGuid { get; set; } = Guid.NewGuid();
        public int? AppUserID { get; set; }
        public AppUser? AppUser { get; set; }





    }
}
