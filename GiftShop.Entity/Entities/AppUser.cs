

using System.ComponentModel.DataAnnotations;

namespace GiftShop.Entity.Entities
{
    public class AppUser
    {
        public int AppUserID { get; set; }

        [Display(Name = "Adı")]
        public string Name { get; set; }

        [Display(Name = "Soyadı")]
        public string? Surname { get; set; }
        public string Email { get; set; }
        [Display(Name = "Şifre")]
        public string Password { get; set; }
        [Display(Name = "Telefon")]
        public string? Phone { get; set; }
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Display(Name = "Admin mi?")]
        public bool IsAdmin { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();

    }
}
