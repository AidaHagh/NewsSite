using AutoMapper;
using Data.ViewModel;
using Entities.Setting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.UnitOfWork;

namespace NewsSite.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "Manage")]
    public class ContactUsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ContactUsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        #region Index

        [HttpGet]
        public IActionResult Index()
        {

            var model = _unitOfWork.contactUsUW.Get();
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
        public IActionResult Create(ContactUsViewModel model)
        {
            if (ModelState.IsValid)
            {

                var mapModel = _mapper.Map<ContactUs>(model);
                _unitOfWork.contactUsUW.Create(mapModel);
                TempData["SuccessMessage"] = "ذخیره اطلاعات با موفقیت انجام شد";
                _unitOfWork.save();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        #endregion



        #region Edit

        [HttpGet]
        public IActionResult Edit(int ContactUsId)
        {
            if (ContactUsId == 0)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";
            }

            var cat = _unitOfWork.contactUsUW.GetById(ContactUsId);
            var mapContactUs = _mapper.Map<ContactUsViewModel>(cat);

            return View(mapContactUs);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(ContactUsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mapModel = _mapper.Map<ContactUs>(model);
                _unitOfWork.contactUsUW.Update(mapModel);
                _unitOfWork.save();

                TempData[SuccessMessage] = "ویرایش اطلاعات با موفقیت انجام شد";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }


        #endregion



        #region Delete

        [HttpGet]
        public IActionResult Delete(int ContactUsId)
        {
            if (ContactUsId == 0)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";
            }

            var cat = _unitOfWork.contactUsUW.GetById(ContactUsId);

            if (ContactUsId == 0)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";
            }

            return PartialView("_deleteContactUs", cat);
        }



        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult DeletePost(int ContactUsId)
        {
            if (ContactUsId == 0)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";
            }
            else
            {
                try
                {
                    _unitOfWork.contactUsUW.DeleteById(ContactUsId);
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
