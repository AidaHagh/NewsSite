using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Data.ViewModel
{
    public class UsersViewModel
    {
        //public string Id { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "نام کاربری وارد نشده است.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "تعداد کاراکترها باید بین 5 تا 50 حرف باشد")]
        public string UserName { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "کلمه عبور وارد نشده است.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "تعداد کاراکترها باید بین 5 تا 50 حرف باشد")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "کلمه عبور وارد نشده است.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "تعداد کاراکترها باید بین 5 تا 50 حرف باشد")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "رمز عبور با تکرار آن یکسان نیست")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "نام")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "نام وارد نشده است.")]
        public string FirstName { get; set; }

        [Display(Name = "فامیلی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "نام خانوادگی وارد نشده است.")]
        public string LastName { get; set; }

        [Display(Name = "جنسیت")]
        public byte gender { get; set; }

        [Display(Name = "شماره تماس")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "شماره تماس وارد نشده است.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "شماره تماس 11 رقمی می باشد")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "شماره تماس شامل حرف نمی تواند باشد")]
        public string PhoneNumber { get; set; }

        [Display(Name = "ایمیل")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ایمیل وارد نشده است.")]
        public string Email { get; set; }

        [Display(Name = "تصویر")]
        public string? ImagePath { get; set; }

        [Display(Name = "تاریخ تولد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "تاریخ تولد وارد نشده است.")]
        public DateTime BirthDayDate { get; set; }
    }


    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "نام کاربری وارد نشده است.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "تعداد کاراکترها باید بین 5 تا 50 حرف باشد")]
        public string UserName { get; set; }

        [Display(Name = "نام")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "نام وارد نشده است.")]
        public string FirstName { get; set; }

        [Display(Name = "فامیلی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "نام خانوادگی وارد نشده است.")]
        public string LastName { get; set; }

        [Display(Name = "جنسیت")]
        public byte gender { get; set; }

        [Display(Name = "شماره تماس")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "شماره تماس وارد نشده است.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "شماره تماس 11 رقمی می باشد")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "شماره تماس شامل حرف نمی تواند باشد")]
        public string PhoneNumber { get; set; }

        [Display(Name = "ایمیل")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ایمیل وارد نشده است.")]
        public string Email { get; set; }

        [Display(Name = "تصویر")]
        public string? ImagePath { get; set; }

        [Display(Name = "تاریخ تولد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "تاریخ تولد وارد نشده است.")]
        public DateTime BirthDayDate { get; set; }
    }


    public class LoginViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "نام کاربری وارد نشده است.")]
        public string UserName { get; set; }


        [Display(Name = "کلمه عبور")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "کلمه عبور وارد نشده است.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "مرا بخاطر بسپار")]
        public bool RememberMe { get; set; }
    }


    public class ChangePasswordByAdminViewModel
    {
        [Display(Name = "رمز عبور جدید")]
        public string NewPassword { get; set; }


        [Display(Name = "تکرار رمز عبور جدید")]
        public string ConfirmNewPassword { get; set; }


        public string userId { get; set; }
    }
}

