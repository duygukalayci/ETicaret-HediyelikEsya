using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop.Entity.Entities
{
    public class ContactMessage
    {
        public int ContactMessageID { get; set; }
        [Display(Name = "Adı")]
        public string? Name { get; set; }
        public string Email { get; set; }
        [Display(Name = "Konu")]
        public string Subject { get; set; }
        [Display(Name = "Mesaj")]
        public string Message { get; set; }
        [Display(Name = "Kayıt Tarihi"),ScaffoldColumn(false)]
        public DateTime? CreatedAt { get; set; } 
        [Display(Name = "Okundu mu?")]
        public bool IsRead { get; set; } 
    }
}
