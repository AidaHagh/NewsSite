using AutoMapper;
using Common.GenericClase;
using Common.UploudImage;
using Data.ViewModel;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Services.UnitOfWork;

namespace NewsSite.Areas.AdminPanel.Controllers;


[Area("AdminPanel")]
[Authorize(Roles = "User")]
public class UserController : BaseController
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUploadFiles _upload;
    private readonly UserManager<ApplicationUsers> _userManager;
    public UserController(IUnitOfWork unitOfWork, IMapper mapper, IUploadFiles upload,
        UserManager<ApplicationUsers> userManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _upload = upload;
        _userManager = userManager;
    }



    #region Index

    [HttpGet]
    public IActionResult Index(byte searchTypeSelected = 0, string textSearch="", int page = 1)
    {
     

        int paresh = (page - 1) * 5;
        int totalCount = _unitOfWork.userUW.Get().Count();
        ViewBag.PageID = page;
        double remain = totalCount % 5;
        if (remain == 0)
        {
            ViewBag.PageCount = totalCount / 5;
        }
        else
        {
            ViewBag.PageCount = (totalCount / 5) + 1;
        }
        ViewBag.searchTypeSelected = searchTypeSelected;
        ViewBag.textSearch = textSearch;

        var model = _unitOfWork.userUW.Get().Skip(paresh).Take(5);
        return View(model);
    }

    #endregion


    #region UserDetails

    [HttpGet]
    public IActionResult UserDetails(string userId)
    {
        var model = _unitOfWork.userUW.GetById(userId);
        return View(model);
    }

    #endregion


    #region UploadImageFile

    [HttpPost]
    public IActionResult UploadImageFile(IEnumerable<IFormFile> filearray, string path)
    {
        string filename = _upload.UploadFileFunc(filearray, path);
        return Json(new { status = "success", imagename = filename });
    }

    #endregion


    #region Create

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UsersViewModel model, string birthday, byte r1, string newImagePathName)
    {
        if (birthday == null)
        {
            TempData["WarningMessage"] = "لطفا تاریخ تولد را وارد نمایید";
            return View(model);
        }

        if (ModelState.IsValid)
        {
            try
            {
                //کنترل تکراری نبودن نام کاربر
                if (await _userManager.FindByNameAsync(model.UserName) != null)
                {
                    TempData["WarningMessage"] = ".نام کاربری تکراری می باشد";
                    return View(model);
                }

                model.BirthDayDate = ConvertDateTime.ConvertShamsiToMiladi(birthday);
                var userMapped = _mapper.Map<ApplicationUsers>(model);
                userMapped.gender = r1;
                userMapped.ImagePath = newImagePathName;
                userMapped.IsActive = true;
                IdentityResult result = await _userManager.CreateAsync(userMapped, model.Password);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "ذخیره اطلاعات با موفقیت انجام شد";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                TempData["WarningMessage"] = "خطایی رخ داده است";
                return View(model);
            }
        }

        return View(model);
    }
    #endregion


    #region Edit

    [HttpGet]
    public IActionResult Edit(string userId)
    {
        if (userId == null)
        {
            TempData["WarningMessage"] = "کاربری یافت نشد";
        }
        var user = _unitOfWork.userUW.GetById(userId);
        var mapUser = _mapper.Map<EditUserViewModel>(user);
        return View(mapUser);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditUserViewModel model, string birthday, byte r1, string newImagePathName)
    {
        if (birthday == null)
        {
            TempData["WarningMessage"] = "لطفا تاریخ تولد را وارد نمایید";
            return View(model);
        }
        model.gender = r1;
        model.ImagePath = newImagePathName;

        if (ModelState.IsValid)
        {
            model.BirthDayDate = ConvertDateTime.ConvertShamsiToMiladi(birthday);
            //update
            var user = await _userManager.FindByIdAsync(model.Id);
            IdentityResult result = await _userManager.UpdateAsync(_mapper.Map(model, user));
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "ویرایش اطلاعات با موفقیت انجام شد";
                return RedirectToAction("Index", "User");
            }
        }
        return View(model);
    }

    #endregion


    #region ActiveOrDeactive

    [HttpGet]
    public IActionResult ActiveOrDeactiveUser(string userID, bool IsActive)
    {
        if (userID == null)
        {
            TempData["WarningMessage"] = "خطایی رخ داده است";

        }
        var user = _unitOfWork.userUW.GetById(userID);
        if (user == null)
        {
            TempData["WarningMessage"] = "خطایی رخ داده است";
        }

        if (user.IsActive == true)
        {
            //DeActive
            ViewBag.theme = "firebrick";
            ViewBag.ViewTitle = "غیرفعال کردن کاربر";
            return PartialView("_ActiveOrDeactiveUser", user);
        }
        else
        {
            //Active
            ViewBag.theme = "green";
            ViewBag.ViewTitle = "فعال کردن کاربر";
            return PartialView("_ActiveOrDeactiveUser", user);
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult ActiveOrDeactiveUserPost(string Id, bool IsActive)
    {
        if (Id == null)
        {
            TempData["WarningMessage"] = "خطایی رخ داده است";
            return RedirectToAction("Index", "User");
        }
        else
        {
            try
            {
                if (IsActive == true)
                {
                    //Deactive
                    var user = _unitOfWork.userUW.GetById(Id);
                    user.IsActive = false;
                    _unitOfWork.userUW.Update(user);
                    _unitOfWork.save();
                }
                else
                {
                    //Active
                    var user = _unitOfWork.userUW.GetById(Id);
                    user.IsActive = true;
                    _unitOfWork.userUW.Update(user);
                    _unitOfWork.save();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["WarningMessage"] = "خطایی رخ داده است";
                return RedirectToAction("Index", "User");
            }

        }
    }
    #endregion


    #region SearchUser

    public IActionResult SearchUser(byte searchTypeSelected = 0, string textSearch = "")
    {
        ViewBag.searchTypeSelected = searchTypeSelected;
        ViewBag.textSearch = textSearch;

        var model = _unitOfWork.userUW.Get();

        if (searchTypeSelected == 1 && textSearch != null)
        {
            model = model.Where(x => x.FirstName.Contains(textSearch)).ToList();
        }

        if (searchTypeSelected == 2 && textSearch != null)
        {
            model = model.Where(x => x.LastName.Contains(textSearch)).ToList();
        }

        if (searchTypeSelected == 3 && textSearch != null)
        {
            model = model.Where(x => x.PhoneNumber.Contains(textSearch)).ToList();
        }

        if (searchTypeSelected == 4 && textSearch != null)
        {
            model = model.Where(x => x.Email.Contains(textSearch)).ToList();
        }

        if (searchTypeSelected == 5 && textSearch != null)
        {
            model = model.Where(x => x.UserName.Contains(textSearch)).ToList();
        }

        return View("Index", model);
    }
    #endregion


    #region ChangePasswordByAdmin

    [HttpGet]
    public IActionResult ChangePasswordByAdmin(string userId, string FullName)
    {
        if (userId == null)
        {
            TempData["WarningMessage"] = "کاربری یافت نشد";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.userId = userId;
        ViewBag.FullName = FullName;
        return PartialView("_ChangePasswordByAdmin");
    }

    [HttpPost]
    public IActionResult ChangePassByAdmin(ChangePasswordByAdminViewModel model)
    {
        try
        {
            var user = _unitOfWork.userUW.Get(u => u.Id == model.userId).FirstOrDefault();
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);
            _unitOfWork.save();

            return Json(new { status = "ok" });
        }
        catch
        {
            return Json(new { status = "error" });
        }
    }
    #endregion

}

