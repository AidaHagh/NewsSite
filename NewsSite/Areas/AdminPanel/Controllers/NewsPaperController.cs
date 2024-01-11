using AutoMapper;
using Common.GenericClase;
using Common.UploudImage;
using Data.ViewModel;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Services.UnitOfWork;
using NewsSite.Areas.AdminPanel.Controllers;
using Entities.news;
using Microsoft.AspNetCore.Authorization;

namespace NewsPaperSite.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "News")]
    public class NewsPaperController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUploadFiles _upload;
        private readonly UserManager<ApplicationUsers> _usermanager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public NewsPaperController(IUnitOfWork unitOfWork, IMapper mapper, IUploadFiles upload,
            UserManager<ApplicationUsers> usermanager, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _upload = upload;
            _usermanager = usermanager;
            _webHostEnvironment = webHostEnvironment;
        }


        #region Index

        [HttpGet]
        public IActionResult Index(int page = 1)
        {

            int paresh = (page - 1) * 5;
            int totalCount = _unitOfWork.newsPaperUW.Get(u => u.UserId == _usermanager.GetUserId(User)).Count();
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

            var model = _unitOfWork.newsPaperUW.Get(u => u.UserId == _usermanager.GetUserId(User)).Skip(paresh).Take(5);
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
            ViewBag.UserID = _usermanager.GetUserId(User);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NewsPaperViewModel model, string newImageName, string newsPaperDate, string newsPaperTime)
        {
            ViewBag.UserID = _usermanager.GetUserId(User);


            if (ModelState.IsValid)
            {
                try
                {
                    model.Date = ConvertDateTime.ConvertShamsiToMiladi(newsPaperDate);
                    model.Time = Convert.ToDateTime(newsPaperTime);
                    var mapModel = _mapper.Map<NewsPaper>(model);
                    mapModel.Image = newImageName;
                    _unitOfWork.newsPaperUW.Create(mapModel);
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
        public IActionResult Edit(int NewsPaperId)
        {
            ViewBag.UserID = _usermanager.GetUserId(User);

            if (NewsPaperId == 0)
            {
                TempData["ErrorMessage"] = "خبری یافت نشد";
            }

            var newsPaper = _unitOfWork.newsPaperUW.GetById(NewsPaperId);
            var mapNewsPaper = _mapper.Map<NewsPaperViewModel>(newsPaper);

            return View(mapNewsPaper);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(NewsPaperViewModel model, string newImageName, string newsPaperDate, string newsPaperTime)
        {
            ViewBag.UserID = _usermanager.GetUserId(User);


            if (ModelState.IsValid)
            {
                model.Date = ConvertDateTime.ConvertShamsiToMiladi(newsPaperDate);
                model.Time = Convert.ToDateTime(newsPaperTime);

                model.Image = newImageName;
                var mapModel = _mapper.Map<NewsPaper>(model);
                _unitOfWork.newsPaperUW.Update(mapModel);
                _unitOfWork.save();

                TempData[SuccessMessage] = "ویرایش اطلاعات با موفقیت انجام شد";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        #endregion


        #region Delete

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";
            }

            var newsPaper = _unitOfWork.newsPaperUW.GetById(id);

            if (newsPaper == null)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";
            }

            return PartialView("_deleteNewsPaper", newsPaper);
        }



        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            if (id == 0)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                try
                {
                    //برای حذف تصویر از روت سایت
                    var deleteImg = _unitOfWork.newsPaperUW.GetById(id);
                    var pathImageName = Path.Combine(_webHostEnvironment.WebRootPath, "upload\\NewsPaperImage\\") + deleteImg.Image;

                    if (System.IO.File.Exists(pathImageName))
                    {
                        System.IO.File.Delete(pathImageName);
                    }
                    _unitOfWork.newsPaperUW.DeleteById(id);
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
