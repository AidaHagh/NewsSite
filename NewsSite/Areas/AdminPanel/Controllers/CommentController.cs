using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Services.Comment;
using Service.Services.UnitOfWork;

namespace NewsSite.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize]
    public class CommentController : BaseController

    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        public CommentController(IUnitOfWork unitOfWork, IMapper mapper, ICommentService commentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _commentService = commentService;
        }





        #region Index

        [HttpGet]
        public IActionResult Index(byte searchTypeSelected = 0, string textSearch = "", int page = 1)
        {


            int paresh = (page - 1) * 5;
            int totalCount = _unitOfWork.commentUW.Get().Count();
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

            var model = _unitOfWork.commentUW.Get(null,null, "News").Skip(paresh).Take(5);
            return View(model);
        }

        #endregion


        #region SearchComment

        public IActionResult SearchComment(byte searchTypeSelected = 0, string textSearch = "")
        {
            ViewBag.searchTypeSelected = searchTypeSelected;
            ViewBag.textSearch = textSearch;

            var model = _unitOfWork.commentUW.Get(null,null, "News");
            if (searchTypeSelected==0 || textSearch==null)
            {
                TempData["WarningMessage"] = "ابتدا گزینه ای برای جست جو انتخاب کرده سپس متن مورد نظر را در کادر جست جو وارد کنید ";
            }

            if (searchTypeSelected == 1 && textSearch!=null)
            {
                model=model.Where(x=>x.FullName.Contains(textSearch)).ToList(); 
            }

            if (searchTypeSelected == 2 && textSearch != null)
            {
                model = model.Where(x => x.Email.Contains(textSearch)).ToList();
            } 

            if (searchTypeSelected == 3 && textSearch != null)
            {
                model = model.Where(x => x.IP.Contains(textSearch)).ToList();
            }  
            
            if (searchTypeSelected == 4 && textSearch != null)
            {
                model = model.Where(x => x.CommentDate.ToString().Contains(textSearch)).ToList();
            }  
            if (searchTypeSelected == 5 && textSearch != null)
            {
                model = model.Where(x => x.CommentTime.ToString().Contains(textSearch)).ToList();
            }

            if (searchTypeSelected == 6 && textSearch != null)
            {
                model = model.Where(x => x.News.Title.Contains(textSearch)).ToList();
            } 
            
            if (searchTypeSelected == 7 && textSearch != null)
            {
                model = model.Where(x => x.Status.ToString().Contains(textSearch)).ToList();
            }   
            
            if (searchTypeSelected == 8 && textSearch != null)
            {
                model = model.Where(x => x.Message.Contains(textSearch)).ToList();
            }

            return View("Index", model);
        }

        #endregion


        #region ActiveOrDeactive

        [HttpGet]
        public IActionResult AcceptOrReject(int? id)
        {
            if (id == null)
            {
                TempData["WarningMessage"] = "خطایی رخ داده است";

            }
            var comment = _unitOfWork.commentUW.GetById(id);

            if (comment == null)
            {
                TempData["WarningMessage"] = "خطایی رخ داده است";
            }

            if (comment.Status == true)
            {
                //Reject
                ViewBag.theme = "goldenrod";
                ViewBag.ViewTitle = "عدم نمایش نظر";
                return PartialView("_AcceptOrRejectComment", comment);
            }
            else
            {
                //Accept
                ViewBag.theme = "green";
                ViewBag.ViewTitle = "تایید و نمایش نظر";
                return PartialView("_AcceptOrRejectComment", comment);
            }
             
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AcceptOrReject(int id)
        {
           _commentService.AcceptOrReject(id);
            return RedirectToAction(nameof(Index));
        }
        #endregion


        #region DeleteComment

        [HttpGet]
        public IActionResult DeleteComment(int id)
        {
            if (id == 0)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";
            }

            var comment = _unitOfWork.commentUW.GetById(id);
            if (id == 0)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";
            }

            return PartialView("_delete", comment);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult DeleteCommentPost(int id)
        {
            if (id == 0)
            {
                TempData["ErrorMessage"] = "خطایی رخ داده است";
            }
            else
            {
                try
                {
                    _unitOfWork.commentUW.DeleteById(id);
                    _unitOfWork.save();
                    TempData["WarningMessage"] = "حذف اطلاعات با موفقیت انجام شد";
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
