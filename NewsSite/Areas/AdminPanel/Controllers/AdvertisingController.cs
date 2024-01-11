using AutoMapper;
using Common.GenericClase;
using Common.UploudImage;
using Data.ViewModel;
using Entities;
using Entities.news;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Services.Advertise;
using Service.Services.UnitOfWork;

namespace NewsSite.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
[Authorize(Roles = "Advertise")]
public class AdvertisingController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUploadFiles _upload;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IAdvertiseService _advertiseService;
    public AdvertisingController(IUnitOfWork unitOfWork, IMapper mapper, IUploadFiles upload,
        IWebHostEnvironment webHostEnvironment, IAdvertiseService advertiseService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _upload = upload;
        _webHostEnvironment = webHostEnvironment;
        _advertiseService = advertiseService;
    }

    #region Index

    [HttpGet]
    public IActionResult Index(int page = 1)
    {

        int paresh = (page - 1) * 5;
        int totalCount = _unitOfWork.adverUW.Get().Count();
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

        var model = _unitOfWork.adverUW.Get().Skip(paresh).Take(5);
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
    public IActionResult Create(AdvertisingViewModel model, string newImagePathName, string fromDateAdv, string toDateAdv)
    {

        if (fromDateAdv == null)
        {
            TempData["WarningMessage"] = "لطفا تاریخ شروع تبلیغ را وارد نمایید";
            return View(model);

        }

        if (toDateAdv == null)
        {
            TempData["WarningMessage"] = "لطفا تاریخ پایان تبلیغ را وارد نمایید";
            return View(model);
        }

        if (ModelState.IsValid)
        {
            try
            {
                model.FromDate = ConvertDateTime.ConvertShamsiToMiladi(fromDateAdv);
                model.ToDate = ConvertDateTime.ConvertShamsiToMiladi(toDateAdv);

                var mapModel = _mapper.Map<Advertising>(model);
                mapModel.GifPath = newImagePathName;
                _unitOfWork.adverUW.Create(mapModel);
                _unitOfWork.save();

                TempData["SuccessMessage"] = "ذخیره اطلاعات با موفقیت انجام شد";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";

                return View(model);
            }
        }

        return View(model);
    }

    #endregion


    #region Edit

    [HttpGet]
    public IActionResult Edit(int Id)
    {

        if (Id == 0)
        {
            TempData["ErrorMessage"] = " یافت نشد";
        }

        var adver = _unitOfWork.adverUW.GetById(Id);

        if (adver == null)
        {
            TempData["ErrorMessage"] = " یافت نشد";
        }
        var mapadver = _mapper.Map<AdvertisingViewModel>(adver);

        return View(mapadver);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Edit(AdvertisingViewModel model, string newImagePathName, string fromDateAdv, string toDateAdv)
    {


        if (ModelState.IsValid)
        {
            model.FromDate = Convert.ToDateTime(fromDateAdv).Date;
            model.ToDate = Convert.ToDateTime(toDateAdv).Date;

            model.GifPath = newImagePathName;
            var mapModel = _mapper.Map<Advertising>(model);
            _unitOfWork.adverUW.Update(mapModel);
            _unitOfWork.save();

            TempData[SuccessMessage] = "ویرایش اطلاعات با موفقیت انجام شد";
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    #endregion


    #region Delete

    [HttpGet]
    public IActionResult Delete()
    {
 
        return PartialView("_deleteAdver");
    }



    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePost(int Id)
    {
        if (Id == 0)
        {
            TempData["ErrorMessage"] = "خطایی رخ داده است";
            return RedirectToAction(nameof(Index));

        }
        else
        {
            try
            {
                //برای حذف تصویر از روت سایت
                var Img = _unitOfWork.adverUW.GetById(Id);
                var pathImageName = Path.Combine(_webHostEnvironment.WebRootPath, "upload\\advImage\\") + Img.GifPath;

                if (System.IO.File.Exists(pathImageName))
                {
                    System.IO.File.Delete(pathImageName);
                }
                _unitOfWork.adverUW.DeleteById(Id);
                _unitOfWork.save();
                TempData[WarningMessage] = "حذف اطلاعات با موفقیت انجام شد";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";
                return RedirectToAction(nameof(Index));

            }
        }

    }

    #endregion


    #region ChangeStatus

    [HttpGet]
    public IActionResult ChangeStatus()
    {
        return PartialView("_changeStatus");
    }

    [HttpPost]
    public IActionResult ChangeStatus(int id)
    {
        _advertiseService.changeStatus(id);
        return RedirectToAction("Index");
    }
    #endregion
}

