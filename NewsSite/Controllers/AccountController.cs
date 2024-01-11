using Data.ViewModel;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsSite.Areas.AdminPanel.Controllers;
using Service.Services.UnitOfWork;

namespace NewsSite.Controllers
{
    public class AccountController : BaseController
    {
        private readonly SignInManager<ApplicationUsers> _signInManager;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public AccountController(SignInManager<ApplicationUsers> signInManager,
            UserManager<ApplicationUsers> userManager, IUnitOfWork unitOfWork)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
 
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    //Success Login
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    var userRole = _userManager.GetRolesAsync(user).Result.ToArray();

                    TempData["SuccessMessage"] = " کاربر گرامی " + user.FirstName + " " + user.LastName + " ورود شما با موفقیت انجام شد! ";

                    return Json(new { status = "success" });

                }
                else
                {
                    //failed Login
                    //اگر اطلاعات کاربر اشتباه بود

                    return Json(new { status = "fail" });
                }
            }

            return Json(new { status = "fail2" });
        }



        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Home");
        }
    }
}
