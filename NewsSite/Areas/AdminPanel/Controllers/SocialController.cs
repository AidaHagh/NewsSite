using AutoMapper;
using Common.GenericClase;
using Common.UploudImage;
using Data.ViewModel;
using Entities;
using Entities.Setting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.UnitOfWork;

namespace NewsSite.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "Manage")]
    public class SocialController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUploadFiles _upload;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SocialController(IUnitOfWork unitOfWork, IMapper mapper, IUploadFiles upload,
            IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _upload = upload;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Index

        [HttpGet]
        public IActionResult Index()
        {
            var model = _unitOfWork.socialUW.Get();
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
        public IActionResult Create(SocialViewModel model, string newImagePathName)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    var mapModel = _mapper.Map<Social>(model);
                    mapModel.Socials_Image = newImagePathName;
                    _unitOfWork.socialUW.Create(mapModel);
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

            var social = _unitOfWork.socialUW.GetById(Id);

            if (social == null)
            {
                TempData["ErrorMessage"] = " یافت نشد";
            }
            var mapsocial = _mapper.Map<SocialViewModel>(social);

            return View(mapsocial);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(SocialViewModel model, string newImagePathName)
        {


            if (ModelState.IsValid)
            {

                model.Socials_Image = newImagePathName;
                var mapModel = _mapper.Map<Social>(model);
                _unitOfWork.socialUW.Update(mapModel);
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

            return PartialView("_deleteSocial");
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
                    var Img = _unitOfWork.socialUW.GetById(Id);
                    var pathImageName = Path.Combine(_webHostEnvironment.WebRootPath, "upload\\socialImage\\") + Img.Socials_Image;

                    if (System.IO.File.Exists(pathImageName))
                    {
                        System.IO.File.Delete(pathImageName);
                    }
                    _unitOfWork.socialUW.DeleteById(Id);
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
    }
}
