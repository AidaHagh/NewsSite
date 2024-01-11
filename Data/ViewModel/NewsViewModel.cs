using Entities.news;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Data.ViewModel
{
    public class NewsViewModel
    {
        public int NewsId { get; set; }

        [Display(Name = "عنوان خبر")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "عنوان خبر وارد نشده است.")]
        [StringLength(150, MinimumLength = 4, ErrorMessage = "تعداد کاراکترها باید بین 4 تا 150 حرف باشد")]
        public string Title { get; set; }

        [Display(Name = "متن خبر")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "متن خبر وارد نشده است.")]
        public string Content { get; set; }

        [Display(Name = "چکیده خبر")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "چکیده خبر وارد نشده است.")]
        public string Abstract { get; set; }

        [Display(Name = "تعداد بازدید")]
        public int VisitCount { get; set; }

        [Display(Name = "تاریخ خبر")]
        public DateTime NewsDate { get; set; }

        [Display(Name = "زمان خبر")]
        public DateTime NewsTime { get; set; }

        [Display(Name = "تصویر خبر")]
        public string? IndexImage { get; set; }

        public string UserId { get; set; }


        [Display(Name = "دسته بندی خبر")]
        public int CategoryId { get; set; }


        [Display(Name = "محل درج خبر")]
        public byte PlaceNewsId { get; set; }


        [Display(Name = " نوع خبر")]
        public byte NewsType { get; set; }


        //SEO Property
        [Display(Name = "متاتگ ها")]
        public string MetaTag { get; set; }
        [Display(Name = "متای توضیحات")]
        public string MetaDescription { get; set; }

    }
}
