using System.ComponentModel.DataAnnotations;

namespace GiftShop.WebUI.Models
{
    public class UserEditViewModel
    {
        public int AppUserID { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Ad zorunludur.")]
        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur.")]
        [Display(Name = "Soyad")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string? Password { get; set; }  // Boş bırakılabilir, doluysa şifre değişir
    }
}


