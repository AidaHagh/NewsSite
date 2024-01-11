using AutoMapper;
using Data.ViewModel;
using Entities.news;
using Entities.Setting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.UnitOfWork;

namespace NewsSite.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "Manage")]
    public class AboutUsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AboutUsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        #region Index

        [HttpGet]
        public IActionResult Index()
        {

            var model = _unitOfWork.aboutUsUW.Get();
            return View(model);
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
        public IActionResult Create(AboutUsViewModel model)
        {
            if (ModelState.IsValid)
            {

                var mapModel = _mapper.Map<AboutUs>(model);
                _unitOfWork.aboutUsUW.Create(mapModel);
                TempData["SuccessMessage"] = "ذخیره اطلاعات با موفقیت انجام شد";
                _unitOfWork.save();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        #endregion



        #region Edit

        [HttpGet]
        public IActionResult Edit(int AboutUsId)
        {
            if (AboutUsId == 0)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";
            }

            var cat = _unitOfWork.aboutUsUW.GetById(AboutUsId);
            var mapAboutUs = _mapper.Map<AboutUsViewModel>(cat);

            return View(mapAboutUs);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(AboutUsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mapModel = _mapper.Map<AboutUs>(model);
                _unitOfWork.aboutUsUW.Update(mapModel);
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

            var cat = _unitOfWork.aboutUsUW.GetById(id);

            if (id == 0)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";
            }

            return PartialView("_deleteAboutUs", cat);
        }



        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            if (id == 0)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";
            }
            else
            {
                try
                {
                    _unitOfWork.aboutUsUW.DeleteById(id);
                    _unitOfWork.save();
                    TempData["InfoMessage"] = "حذف اطلاعات با موفقیت انجام شد";
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "خطایی رخ داده است";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
