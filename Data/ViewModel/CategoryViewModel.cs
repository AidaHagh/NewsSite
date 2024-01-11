using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Data.ViewModel
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }


        [Display(Name = "دسته بندی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "دسته بندی وارد نشده است.")]
        [StringLength(150, MinimumLength = 4, ErrorMessage = "تعداد کاراکترها باید بین 4 تا 150 حرف باشد")]
        public string Title { get; set; }


        [Display(Name = "توضیحات")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "توضیخات وارد نشده است.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "تعداد کاراکترها باید بین 10 تا 500 حرف باشد")]
        public string Description { get; set; }
    }
}
