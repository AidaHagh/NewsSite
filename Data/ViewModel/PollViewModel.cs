using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Data.ViewModel
{
    public class PollViewModel
    {
        [Display(Name = "سوال نظرسنجی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا {0} وارد نمایید.")]
        public string Question { get; set; }

        [Display(Name = "پاسخ ها")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "لطفا {0} وارد نمایید.")]
        public string Answer { get; set; }
    }


    public class VoteResultViewModel
    {
        public int data { get; set; }
        public string label { get; set; }
    }

}
