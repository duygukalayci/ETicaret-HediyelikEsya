

using System.ComponentModel.DataAnnotations;

namespace GiftShop.Entity.Entities
{
    public class AppUser
    {
        public int AppUserID { get; set; }

        [Display(Name = "Adı")]
        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [StringLength(50)] 
        public string Name { get; set; }

        [Display(Name = "Soyadı")]
        public string? Surname { get; set; }
        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; }
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Telefon numarası 11 haneli olmalıdır.")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; }
        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [StringLength(30)]
        public string UserName { get; set; }

        [Display(Name = "Admin mi?")]
        public bool IsAdmin { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Address> Addresses { get; set; } = new List<Address>();


    }
}
