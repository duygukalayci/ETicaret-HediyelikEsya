using System.ComponentModel.DataAnnotations;

namespace GiftShop.WebUI.Models
{
    public class LoginViewModel
    {
        [DataType(DataType.EmailAddress),Required(ErrorMessage ="Email Boş Geçilemez!")]
        public string Email { get; set; }
        [Display(Name = "Şifre"), Required(ErrorMessage = "Şifre Boş Geçilemez!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }


    }
}
