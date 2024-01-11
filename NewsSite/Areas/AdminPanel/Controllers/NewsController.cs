using AutoMapper;
using Common.GenericClase;
using Common.UploudImage;
using Data.ViewModel;
using Entities;
using Entities.news;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Service.Services.UnitOfWork;

namespace NewsSite.Areas.AdminPanel.Controllers
{

    [Area("AdminPanel")]
    [Authorize(Roles = "News")]
    public class NewsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUploadFiles _upload;
        private readonly UserManager<ApplicationUsers> _usermanager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public NewsController(IUnitOfWork unitOfWork, IMapper mapper, IUploadFiles upload,
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
        public IActionResult Index( int page = 1)
        {

            int paresh = (page - 1) * 5;
            int totalCount = _unitOfWork.newsUW.Get(u=>u.UserId==_usermanager.GetUserId(User),null, "Category").Count();
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

            var model = _unitOfWork.newsUW.Get(u => u.UserId == _usermanager.GetUserId(User), null, "Category").Skip(paresh).Take(5);
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
            ViewBag.CategoryList = new SelectList(_unitOfWork.categoryUW.Get(), "CategoryId", "Title");
            ViewBag.UserID = _usermanager.GetUserId(User);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NewsViewModel model, string newIndexImageName, string newsDate, string newsTime)
        {
            ViewBag.CategoryList = new SelectList(_unitOfWork.categoryUW.Get(), "CategoryId", "Title");
            ViewBag.UserID = _usermanager.GetUserId(User);


            if (ModelState.IsValid)
            {
                try
                {
                    model.NewsDate = ConvertDateTime.ConvertShamsiToMiladi(newsDate);
                    model.NewsTime = Convert.ToDateTime(newsTime);
                    var mapModel = _mapper.Map<News>(model);
                    mapModel.IndexImage = newIndexImageName;
                    _unitOfWork.newsUW.Create(mapModel);
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
        public IActionResult Edit(int NewsId)
        {
            ViewBag.CategoryList = new SelectList(_unitOfWork.categoryUW.Get(), "CategoryId", "Title");
            ViewBag.UserID = _usermanager.GetUserId(User);

            if (NewsId == 0)
            {
                TempData["ErrorMessage"] = "خبری یافت نشد";
            }

            var news = _unitOfWork.newsUW.GetById(NewsId);
            var mapNews = _mapper.Map<NewsViewModel>(news);

            return View(mapNews);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(NewsViewModel model, string newIndexImageName, string newsDate, string newsTime)
        {
            ViewBag.CategoryList = new SelectList(_unitOfWork.categoryUW.Get(), "CategoryId", "Title");
            ViewBag.UserID = _usermanager.GetUserId(User);


            if (ModelState.IsValid)
            {
                model.NewsDate = ConvertDateTime.ConvertShamsiToMiladi(newsDate);
                model.NewsTime = Convert.ToDateTime(newsTime);

                model.IndexImage = newIndexImageName;
                var mapModel = _mapper.Map<News>(model);
                _unitOfWork.newsUW.Update(mapModel);
                _unitOfWork.save();

                TempData[SuccessMessage] = "ویرایش اطلاعات با موفقیت انجام شد";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        #endregion


        #region Delete

        [HttpGet]
        public IActionResult Delete(int NewsId)
        {
            if (NewsId == 0)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";
            }

            var news = _unitOfWork.newsUW.GetById(NewsId);

            if (news == null)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";
            }

            return PartialView("_deleteNews", news);
        }



        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult DeletePost(int NewsId)
        {
            if (NewsId == 0)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                try
                {
                    //برای حذف تصویر از روت سایت
                    var deleteImg = _unitOfWork.newsUW.GetById(NewsId);
                    var pathImageName = Path.Combine(_webHostEnvironment.WebRootPath, "upload\\indexImage\\") + deleteImg.IndexImage;
                  
                    if (System.IO.File.Exists(pathImageName))
                    {
                        System.IO.File.Delete(pathImageName);
                    }
                    _unitOfWork.newsUW.DeleteById(NewsId);
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
