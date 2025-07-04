using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftShop.Entity.Entities
{
    public class Slider
    {
        public int SliderID { get; set; }
        [Display(Name = "Resim")]
        public string? ImageUrl { get; set; }
        [Display(Name = "Başlık")]
        public string Title { get; set; }


    }
}
