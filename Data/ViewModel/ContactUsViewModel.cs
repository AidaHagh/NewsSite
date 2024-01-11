using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Data.ViewModel
{
    public class ContactUsViewModel
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "شماره تماس")]
        [MaxLength(40, ErrorMessage = "مقدار {0} نباید بیش تر از {1} کاراکتر باشد")]
        public string Tel { get; set; }

        [Display(Name = "ایمیل")]
        [MaxLength(40, ErrorMessage = "مقدار {0} نباید بیش تر از {1} کاراکتر باشد")]
        public string Email { get; set; }
    }
}
