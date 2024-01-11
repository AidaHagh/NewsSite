using Data.ViewModel;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsSite.Models;
using Newtonsoft.Json;
using Service.Services.Comment;
using Service.Services.News;
using Service.Services.Poll;
using Service.Services.UnitOfWork;
using System.Diagnostics;

namespace NewsSite.Controllers;

public class HomeController : Controller
{
    private readonly SignInManager<ApplicationUsers> _signInManager;
    private readonly UserManager<ApplicationUsers> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INewsService _newsService;
    private readonly ICommentService _commentServic;
    private readonly IPollService _pollService;

    public HomeController(SignInManager<ApplicationUsers> signInManager,
        UserManager<ApplicationUsers> userManager, IUnitOfWork unitOfWork,
        INewsService newsService, ICommentService commentServic, IPollService pollService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _newsService = newsService;
        _commentServic = commentServic;
        _pollService = pollService;
    }



    #region Index

    public IActionResult Index()
    {
        if (_signInManager.IsSignedIn(User))
        {
            var query = _unitOfWork.userUW.GetById(_userManager.GetUserId(HttpContext.User));
            ViewBag.FullName = query.FirstName + " " + query.LastName;
        }

        var model = new IndexModel();

        model.SliderNews = _unitOfWork.newsUW.Get(s => s.PlaceNewsId == 1).OrderByDescending(n => n.NewsId).Take(3).ToList();
        model.SpecialNews = _unitOfWork.newsUW.Get(s => s.PlaceNewsId == 2).OrderByDescending(n => n.NewsId).Take(10).ToList();
        model.LastVideo = _unitOfWork.newsUW.Get(s => s.PlaceNewsId == 4).OrderByDescending(n => n.NewsId).Take(6).ToList();

        model.LastNews = _unitOfWork.newsUW.Get().OrderByDescending(n => n.NewsId).Take(16).ToList();
        model.InternalNews = _unitOfWork.newsUW.Get(x => x.NewsType == 1).OrderByDescending(n => n.NewsId).Take(16).ToList();
        model.ForeignNews = _unitOfWork.newsUW.Get(x => x.NewsType == 2).OrderByDescending(n => n.NewsId).Take(16).ToList();
        model.privateNews = _unitOfWork.newsUW.Get(x => x.NewsType == 3).OrderByDescending(n => n.NewsId).Take(16).ToList();
        model.social = _unitOfWork.socialUW.Get().ToList();

        model.GetNewsPapers = _unitOfWork.newsPaperUW.Get().OrderByDescending(x => x.Date).Take(3).ToList();

        model.Adver = _unitOfWork.adverUW.Get(a => (a.FromDate.CompareTo(DateTime.Today) <= 0
            && a.ToDate.CompareTo(DateTime.Today) >= 0) && a.Flag == 0).ToList();

        if (_unitOfWork.pollUW.Get(p => p.Active == true).Count() == 1)
        {
            var pollresult = _unitOfWork.pollUW.Get(p => p.Active == true).Single();
            if (Request.Cookies["poll" + pollresult.PollId] == null)
            {

                //کاربر هنوز در نظرسنجی شرکت نکرده است
                model.PollModel = pollresult;
            }
            else
            {
                //کاربر در نظرسنجی شرکت کرده است
                //نمایش نتایج به صورت نمودار
                List<VoteResultViewModel> voteresult = new List<VoteResultViewModel>();
                foreach (PollOption vr in _unitOfWork.pollOptionUW.Get(p => p.PollID == pollresult.PollId))
                {
                    voteresult.Add(new VoteResultViewModel
                    {
                        data = vr.VouteCount,
                        label = vr.Answer
                    });
                }
                model.PollModel = pollresult;
                ViewBag.getListofAnswer = JsonConvert.SerializeObject(voteresult);
            }
        }
        else
        {
            //نظر سنجی فعالی وجود ندارد
            model.PollModel = null;
        }

        return View(model);
    }
    #endregion



    #region NewsDetail
    public IActionResult NewsDetails(int Id)
    {
        var model = new IndexModel();

        model.LastNews = _unitOfWork.newsUW.Get().OrderByDescending(n => n.NewsId).Take(16).ToList();
        model.InternalNews = _unitOfWork.newsUW.Get(x => x.NewsType == 1).OrderByDescending(n => n.NewsId).Take(16).ToList();
        model.ForeignNews = _unitOfWork.newsUW.Get(x => x.NewsType == 2).OrderByDescending(n => n.NewsId).Take(16).ToList();
        model.privateNews = _unitOfWork.newsUW.Get(x => x.NewsType == 3).OrderByDescending(n => n.NewsId).Take(16).ToList();
        model.social = _unitOfWork.socialUW.Get().ToList();
        model.GetNewsPapers = _unitOfWork.newsPaperUW.Get().OrderByDescending(x => x.Date).Take(3).ToList();

        model.Adver = _unitOfWork.adverUW.Get(a => (a.FromDate.CompareTo(DateTime.Today) <= 0
      && a.ToDate.CompareTo(DateTime.Today) >= 0) && a.Flag == 0).ToList();

        var newsinfo = _unitOfWork.newsUW.GetById(Id);

        ViewBag.NewsDetails = _unitOfWork.newsUW.GetById(Id);
        ViewBag.comments = _unitOfWork.commentUW.Get(x => x.NewsId == Id && x.Status == true);

        //ارسال اطلاعات متاتگ ها
        ViewData["metaKey"] = newsinfo.MetaTag;
        ViewData["metadescription"] = newsinfo.MetaDescription;
        //ViewData["Title"] = newsinfo.Title;

        //دستورات مربوط به افزایش آمار بازدید
        _newsService.RefreshVisitorCount(Id);

        return View(model);
    }

    #endregion



    #region InsertComment

    [HttpPost]
    public IActionResult InsertComment(string txtEmail, string txtFullName, string txtMessage, int newsid, int cmid)
    {
        if (txtEmail == null || txtFullName == null || txtMessage == null)
        {
            return Json(new { status = "failModel" });
        }

        try
        {
            Comment model = new Comment();
            model.Email = txtEmail;
            model.FullName = txtFullName;
            model.Message = txtMessage;
            model.CommentDate = DateTime.Now;
            model.CommentTime = DateTime.Now;
            model.Status = false;
            model.NewsId = newsid;
            model.IP = HttpContext.Connection.RemoteIpAddress.ToString();

            model.ReplyID = cmid;

            _unitOfWork.commentUW.Create(model);
            _unitOfWork.save();

            return Json(new { status = "success" });
        }
        catch (Exception)
        {
            return Json(new { status = "fail" });
        }
    }

    #endregion



    #region Like

    [HttpPost]
    public async Task<IActionResult> Like(int cmid)
    {
        var IsCommentExist = _unitOfWork.commentUW.GetById(cmid);

        if (IsCommentExist == null)
        {
            //اگر آی دی کامنت اشتباه بود
            //return null;
            return Redirect(Request.Headers["Referer"].ToString());
        }

        //چک کردن اینکه آیا از قبل کوکی ایجاد شده است یا نه
        if (Request.Cookies["commentLike"] == null)
        {
            //کوکی وجود نداشته است
            //پس کوکی باید ایجاد شود
            Response.Cookies.Append("commentLike", "," + cmid + ",",
                //دوره حیات کوکی
                new Microsoft.AspNetCore.Http.CookieOptions() { Expires = DateTime.Now.AddYears(1) });
            _commentServic.IncreaseLike(cmid);

            return Json(new { status = "success", result = IsCommentExist.LikeCount, backid = cmid });

        }
        else
        {
            //اگر کوکی از قبل وجود داشت
            //باید محتویات یک کوکی را بدست آورد
            string cookieContent = Request.Cookies["commentLike"].ToString();
            //اگر کاربر خواست یک کامنت را 2 بار لایک کند
            if (cookieContent.Contains("," + cmid + ","))
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
            else
            {
                //اگر کاربر قبلا کامنتی را لایک کرده است و حالا می خواهد کامنت دیگری را لایک کند
                cookieContent += "," + cmid + ",";
                //اپیند یعنی کوکی را از بین میبریم و دوباره میسازیم با محتویات جدید
                Response.Cookies.Append("commentLike", cookieContent,
                    new Microsoft.AspNetCore.Http.CookieOptions() { Expires = DateTime.Now.AddYears(1) });

                _commentServic.IncreaseLike(cmid);
                return Json(new { status = "success", result = IsCommentExist.LikeCount, backid = cmid });
            }

        }

    }

    #endregion



    #region DisLike

    [HttpPost]
    public async Task<IActionResult> DisLike(int cmid)
    {
        var IsCommentExist = _unitOfWork.commentUW.GetById(cmid);
        if (IsCommentExist == null)
        {
            //اگر آی دی کامنت اشتباه بود
            //return null;
            return Redirect(Request.Headers["Referer"].ToString());
        }

        //چک کردن اینکه آیا از قبل کوکی ایجاد شده است یا نه
        if (Request.Cookies["commentDisLike"] == null)
        {
            //کوکی وجود نداشته است
            //پس کوکی باید ایجاد شود
            Response.Cookies.Append("commentDisLike", "," + cmid + ",",
                new Microsoft.AspNetCore.Http.CookieOptions() { Expires = DateTime.Now.AddYears(1) });

            _commentServic.IncreasedisLike(cmid);

            return Json(new { status = "success", result = IsCommentExist.DisLikeCount, backid = cmid });
        }
        else
        {
            //اگر کوکی از قبل وجود داشت

            string cookieContent = Request.Cookies["commentDisLike"].ToString();
            //اگر کاربر خواست یک کامنت را 2 بار لایک کند
            if (cookieContent.Contains("," + cmid + ","))
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
            else
            {
                //اگر کاربر قبلا کامنتی را لایک کرده است و حالا می خواهد کامنت دیگری را لایک کند
                cookieContent += "," + cmid + ",";
                Response.Cookies.Append("commentDisLike", cookieContent,
                     new Microsoft.AspNetCore.Http.CookieOptions() { Expires = DateTime.Now.AddYears(1) });

                _commentServic.IncreasedisLike(cmid);
                return Json(new { status = "success", result = IsCommentExist.DisLikeCount, backid = cmid });
            }

        }
    }
    #endregion



    #region setVote

    [HttpPost]
    public IActionResult setVote(int answerid, int pollid)
    {
        if (answerid != 0 && pollid != 0)
        {
            //یعنی کاربر یک گزینه را انتخاب کرده است
            if (Request.Cookies["poll" + pollid] == null)
            {
                //یعنی کاربر اولین بار است که می خواهد در این رای گیری شرکت نماید
                Response.Cookies.Append("poll" + pollid, "kjdhsfkshfsdkjfhs",
                    new Microsoft.AspNetCore.Http.CookieOptions() { Expires = DateTime.Now.AddMonths(1) });

                //ثبت رای
                _pollService.setVote(answerid);
                return Json(new { status = "success" });
            }
            else
            {
                //کاربر قبلا رای داده است
                return Json(new { status = "fail" });
            }
        }
        else
        {
            //کاربر هیچ گزینه ای را انتخاب نکرده است و روی ثبت نظر کلیک کرده است
            return Json(new { status = "fail" });
        }
    }
    #endregion



    #region SearchResult

    [HttpGet]
    public IActionResult SearchResult(string txtsearch)
    {
        var model = new IndexModel();
        model.Adver = _unitOfWork.adverUW.Get(a => (a.FromDate.CompareTo(DateTime.Now) <= 0
        && a.ToDate.CompareTo(DateTime.Now) >= 0) && a.Flag == 0).ToList();

        model.social = _unitOfWork.socialUW.Get().ToList();

        if (txtsearch != null)
        {
            //اخباری را پیدا کن که تایتل آنها شامل تکتس سرچ باشد
            model.searchmodel = _unitOfWork.newsUW.Get(n => n.Title.Contains(txtsearch) || n.Abstract.Contains(txtsearch)).ToList();
            if (model.searchmodel.Count() > 0)//یکسری خبر را پیدا کرده
            {
                ViewBag.searchVal = txtsearch;
                return View("SearchResult", model);
            }
            else
            {
                model.searchmodel = null;
            }
        }
        return View("SearchResult", model);
    }
    #endregion



    #region MenuResult
    [HttpGet]
    public IActionResult MenuResult(int? id, int page = 1)
    {
        int paresh = (page - 1) * 8;
        int totalCount = _unitOfWork.newsUW.Get(n => n.CategoryId == id).Count();
        ViewBag.PageID = page;
        double remain = totalCount % 8;
        if (remain == 0)
        {
            ViewBag.PageCount = totalCount / 8;
        }
        else
        {
            ViewBag.PageCount = (totalCount / 8) + 1;
        }
        var model = new IndexModel();
        model.Adver = _unitOfWork.adverUW.Get(a => (a.FromDate.CompareTo(DateTime.Now) <= 0
        && a.ToDate.CompareTo(DateTime.Now) >= 0) && a.Flag == 0).ToList();

        model.social = _unitOfWork.socialUW.Get().ToList();

        if (id != null)
        {
            //"tblCategory"نوشتن متن
            model.searchmodel = _unitOfWork.newsUW.Get(n => n.CategoryId == id, null, "Category").Skip(paresh).Take(8).ToList();
            if (model.searchmodel.Count() > 0)
            {
                //نوشتنمتن
                ViewBag.menudesc = model.searchmodel[0].Category.Title;
                return View("SearchResult", model);
            }
            else
            {
                model.searchmodel = null;
            }
        }
        return View("SearchResult", model);
    }
    #endregion



    #region AboutUs
    public IActionResult AboutUs()
    {
        var model = new IndexModel();

        model.social = _unitOfWork.socialUW.Get().ToList();

        model.Adver = _unitOfWork.adverUW.Get(a => (a.FromDate.CompareTo(DateTime.Now) <= 0
                 && a.ToDate.CompareTo(DateTime.Now) >= 0) && a.Flag == 0).ToList();

        model.aboutUs = _unitOfWork.aboutUsUW.Get().FirstOrDefault();

        return View(model);
    }
    #endregion



    #region ContactUs
    public IActionResult ContactUs()
    {
        var model = new IndexModel();

        model.LastNews = _unitOfWork.newsUW.Get().OrderByDescending(n => n.NewsId).Take(16).ToList();
        model.InternalNews = _unitOfWork.newsUW.Get(x => x.NewsType == 1).OrderByDescending(n => n.NewsId).Take(16).ToList();
        model.ForeignNews = _unitOfWork.newsUW.Get(x => x.NewsType == 2).OrderByDescending(n => n.NewsId).Take(16).ToList();
        model.privateNews = _unitOfWork.newsUW.Get(x => x.NewsType == 3).OrderByDescending(n => n.NewsId).Take(16).ToList();

        model.social = _unitOfWork.socialUW.Get().ToList();
        model.GetNewsPapers = _unitOfWork.newsPaperUW.Get().OrderByDescending(x => x.Date).Take(3).ToList();

        model.Adver = _unitOfWork.adverUW.Get(a => (a.FromDate.CompareTo(DateTime.Now) <= 0
                 && a.ToDate.CompareTo(DateTime.Now) >= 0) && a.Flag == 0).ToList();

        model.contactUs = _unitOfWork.contactUsUW.Get().FirstOrDefault();

        return View(model);
    }
    #endregion



    #region NewsPaperPage

    public IActionResult NewsPaperPage(int page=1)
    {
        int paresh = (page - 1) * 6;
        int totalCount = _unitOfWork.newsPaperUW.Get().Count();
        ViewBag.PageID = page;
        double remain = totalCount % 6;
        if (remain == 0)
        {
            ViewBag.PageCount = totalCount / 6;
        }
        else
        {
            ViewBag.PageCount = (totalCount / 6) + 1;
        }
        var model = new IndexModel();
        model.GetNewsPapers = _unitOfWork.newsPaperUW.Get().OrderByDescending(x => x.Date).Skip(paresh).Take(6).ToList();
        model.social = _unitOfWork.socialUW.Get().ToList();
        model.Adver = _unitOfWork.adverUW.Get(a => (a.FromDate.CompareTo(DateTime.Now) <= 0
         && a.ToDate.CompareTo(DateTime.Now) >= 0) && a.Flag == 0).ToList();
        return View(model);

    }

    #endregion


    #region MoreVideo

    public IActionResult ShowAllVideo(int page = 1)
    {
        var model = new IndexModel();

        int paresh = (page - 1) * 9;
        int totalCount = _unitOfWork.newsUW.Get(s => s.PlaceNewsId == 4).Count();
        ViewBag.PageID = page;
        double remain = totalCount % 9;
        if (remain == 0)
        {
            ViewBag.PageCount = totalCount / 9;
        }
        else
        {
            ViewBag.PageCount = (totalCount / 9) + 1;
        }

        model.social = _unitOfWork.socialUW.Get().ToList();
        model.Adver = _unitOfWork.adverUW.Get(a => (a.FromDate.CompareTo(DateTime.Now) <= 0
         && a.ToDate.CompareTo(DateTime.Now) >= 0) && a.Flag == 0).ToList();

        model.LastVideo = _unitOfWork.newsUW.Get(s => s.PlaceNewsId == 4).OrderByDescending(n => n.NewsId).Skip(paresh).Take(9).ToList();

        return View(model);

    }

    #endregion
}
