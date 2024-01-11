using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Data.ViewModel
{
    public class SocialViewModel
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "نام ")]
        [MaxLength(200)]
        public string Socials_Name { get; set; }


        [Display(Name = "لینک ")]
        [MaxLength(400)]
        public string Socials_Link { get; set; }


        [Display(Name = "تصویر ")]
        [MaxLength(2000)]
        public string? Socials_Image { get; set; }
    }
}
