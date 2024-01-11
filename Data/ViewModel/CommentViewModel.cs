using Entities.news;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ViewModel
{
    public class CommentViewModel
    {
        [Key]
        public int Id { get; set; }

        public int NewsId { get; set; }


        [Display(Name = "نام")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "نام وارد نشده است.")]
        [StringLength(150, MinimumLength = 4, ErrorMessage = "تعداد کاراکترها باید بین 4 تا 150 حرف باشد")]
        [RegularExpression(@"[0-9A-Zا-یa-z_\s\-\(\)\.]+", ErrorMessage = "در {0} کاراکترهای نامعتبر وارد شده است.")]
        public string FullName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ایمیل وارد نشده است.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "ایمیل را صحیح وارد نمایید")]
        public string Email { get; set; }

        [Display(Name = "متن نظر")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "متن نظر وارد نشده است.")]
        [StringLength(150, MinimumLength = 4, ErrorMessage = "تعداد کاراکترها باید بین 4 تا 150 حرف باشد")]
        [RegularExpression(@"[0-9A-Zا-یa-z_\s\-\(\)\.]+", ErrorMessage = "در {0} کاراکترهای نامعتبر وارد شده است.")]
        public string Message { get; set; }

        [Display(Name = "آی پی")]
        public string IP { get; set; }

        [Display(Name = "تاریخ ارسال نظر")]
        public DateTime CommentDate { get; set; }

        [Display(Name = "زمان ارسال نظر")]
        public DateTime CommentTime { get; set; }

        public int LikeCount { get; set; }
        public int DisLikeCount { get; set; }

        [Display(Name = "وضعیت انتشار")]
        public bool Status { get; set; }

        public int ReplyID { get; set; }


    }
}
