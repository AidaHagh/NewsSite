using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ViewModel
{
    public class NewsPaperViewModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "عنوان روزنامه")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "عنوان روزنامه وارد نشده است.")]
        [StringLength(150, MinimumLength = 4, ErrorMessage = "تعداد کاراکترها باید بین 4 تا 150 حرف باشد")]
        public string Title { get; set; }


        [Display(Name = "تصویر روزنامه")]
        public string? Image { get; set; }


        [Display(Name = "تعداد بازدید")]
        public int VisitCount { get; set; }


        [Display(Name = "تاریخ روزنامه")]
        public DateTime Date { get; set; }


        [Display(Name = "زمان روزنامه")]
        public DateTime Time { get; set; }

        public string UserId { get; set; }



        //SEO Property
        [Display(Name = "متاتگ ها")]
        public string MetaTag { get; set; }
        [Display(Name = "متای توضیحات")]
        public string MetaDescription { get; set; }
    }
}
