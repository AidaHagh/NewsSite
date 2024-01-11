using AutoMapper;
using Data.ViewModel;
using Entities.news;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Services.UnitOfWork;

namespace NewsSite.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "News")]
    public class CategoryController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper) 
        { 
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        #region Index

        [HttpGet]
        public IActionResult Index(string textSearch, int page = 1)
        {
            ViewBag.textSearch = textSearch;

            int paresh = (page - 1) * 5;
            int totalCount = _unitOfWork.categoryUW.Get().Count();
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

            var model = _unitOfWork.categoryUW.Get().Skip(paresh).Take(5);
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
        public IActionResult Create(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var cat = new Category
                //{
                //    Title = model.Title,
                //    Description = model.Description,
                //};
                //TempData["SuccessMessage"] = "ذخیره اطلاعات با موفقیت انجام شد";
                //_unitOfWork.categoryUW.Create(cat);
                //_unitOfWork.save();
                //return RedirectToAction(nameof(Index));

                var mapModel = _mapper.Map<Category>(model);
                _unitOfWork.categoryUW.Create(mapModel);
                TempData["SuccessMessage"] = "ذخیره اطلاعات با موفقیت انجام شد";
                _unitOfWork.save();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        #endregion



        #region Edit

        [HttpGet]
        public IActionResult Edit(int CategoryId)
        {
            if (CategoryId == 0)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";
            }

            var cat = _unitOfWork.categoryUW.GetById(CategoryId);
            var mapCategory = _mapper.Map<CategoryViewModel>(cat);

            return View(mapCategory);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mapModel = _mapper.Map<Category>(model);
                _unitOfWork.categoryUW.Update(mapModel);
                _unitOfWork.save();

                TempData[SuccessMessage] = "ویرایش اطلاعات با موفقیت انجام شد";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }


        #endregion



        #region Delete

        [HttpGet]
        public IActionResult Delete(int CategoryId)
        {
            if (CategoryId == 0)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";
            }

            var cat = _unitOfWork.categoryUW.GetById(CategoryId);

            if (CategoryId == 0)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";
            }

            return PartialView("_deleteCategory", cat);
        }



        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult DeletePost(int CategoryId)
        {
            if (CategoryId == 0)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";
            }
            else
            {
                try
                {
                    _unitOfWork.categoryUW.DeleteById(CategoryId);
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



        #region Search

        public IActionResult SearchCategory(string textSearch = "")
        {
            ViewBag.textSearch = textSearch;

            if (ViewBag.textSearch==null)
            {
                TempData["WarningMessage"] = "مقداری برای جستجو وارد نشده است";
                return RedirectToAction(nameof(Index));
            }

            var model = _unitOfWork.categoryUW.Get();

            model = model.Where(c => c.Title.Contains(textSearch)).ToList();
       
            return View("Index", model);
        }
        #endregion




    }
}
