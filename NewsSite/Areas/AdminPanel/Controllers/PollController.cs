
using Data.ViewModel;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Service.Services.Poll;
using Service.Services.UnitOfWork;

namespace NewsSite.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = "Poll")]
    public class PollController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPollService _pollService;
        public PollController(IUnitOfWork unitOfWork, IPollService pollService)
        {
            _unitOfWork = unitOfWork;
            _pollService= pollService;
        }



        #region Index

        [HttpGet]
        public IActionResult Index(int page = 1)
        {

            int paresh = (page - 1) * 5;
            int totalCount = _unitOfWork.pollUW.Get().Count();
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

            var model = _unitOfWork.pollUW.Get().Skip(paresh).Take(5);
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
        public IActionResult Create(PollViewModel model)
        {
            if (ModelState.IsValid)
            {

                //کنترل اینکه از قبل نظرسنجی فعال موجود نباشد
                var activePoll = _unitOfWork.pollUW.Get(p => p.Active == true);
                if (activePoll.Count() > 0) // یک نظر سنجی فعال وجود دارد
                {
                    ViewBag.Message = "از قبل یک نظرسنجی فعال وجود دارد";
                    return View();
                }
                using (var transaction = _unitOfWork.BeginTransac())

                {
                    try
                    {
                        //ثبت نظرسنجی
                        Poll poll = new Poll();
                        poll.Question = model.Question;
                        poll.Active = true;
                        poll.PollStartDate = DateTime.Now;
                        _unitOfWork.pollUW.Create(poll);
                        _unitOfWork.save();

                        // ثبت پاسخ های نظرسنجی
                        var answerList = model.Answer.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var answer in answerList)
                        {
                            PollOption pollOpt = new PollOption();
                            pollOpt.Answer = answer;
                            pollOpt.VouteCount = 0;
                            pollOpt.PollID = poll.PollId;

                            _unitOfWork.pollOptionUW.Create(pollOpt);
                            _unitOfWork.save();
                        }
                        transaction.Commit();
                        TempData["SuccessMessage"] = "ذخیره اطلاعات با موفقیت انجام شد";
                        return RedirectToAction("Index");

                    }
                    catch (Exception)
                    {
                        transaction.RollBack();
                        TempData["WarningMessage"] = "خطایی رخ داده است";
                        return View(model);
                    }
                }

            }



            return View();
        }

        #endregion


        #region Delete

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Poll poll = _unitOfWork.pollUW.GetById(id);

            if (poll == null)
            {
                return NotFound();
            }

            return PartialView("_delete", poll);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePoll(int id)
        {
            using (var transacation = _unitOfWork.BeginTransac())
            {
                try
                {
                    //حذف پاسخ ها
                    //اول فرزند حذف بعد پدر برعکس ثبت پاسخ
                    var polloption = _unitOfWork.pollOptionUW.Get(po => po.PollID == id).ToList();
                    for (int i = 0; i <= polloption.Count() - 1; i++)
                    {
                        _unitOfWork.pollOptionUW.DeleteById(polloption[i].PolloptionID);
                        _unitOfWork.save();
                    }


                    //حذف نظرسنجی
                    _unitOfWork.pollUW.DeleteById(id);
                    _unitOfWork.save();

                    transacation.Commit();
                    TempData["SuccessMessage"] = "حذف اطلاعات با موفقیت انجام شد";
                    return RedirectToAction("Index");

                }
                catch
                {
                    transacation.RollBack();
                    TempData["WarningMessage"] = "خطایی رخ داده است";
                }

                return RedirectToAction("Index");
            }
        }
        #endregion


        #region ClosePoll

        [HttpGet]
        public IActionResult ClosePoll(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Poll pl = _unitOfWork.pollUW.GetById(id);

            if (pl == null)
            {
                return NotFound();
            }
            return PartialView("_closePoll", pl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClosePoll(int id)
        {
            _pollService.ClosePoll(id);
            return RedirectToAction("Index");
        }


        #endregion


        #region PollResult

        [HttpGet]
        public IActionResult PollResult(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var pollresult = _unitOfWork.pollUW.Get(p => p.PollId == id).Single();

            List<VoteResultViewModel> voteresult = new List<VoteResultViewModel>();
            foreach (PollOption vr in _unitOfWork.pollOptionUW.Get(p => p.PollID == pollresult.PollId))
            {
                voteresult.Add(new VoteResultViewModel
                {
                    data = vr.VouteCount,
                    label = vr.Answer
                });
            }

            ViewBag.getListofAnswer = JsonConvert.SerializeObject(voteresult);
            return PartialView("_pollresult", pollresult);
        }

        #endregion

    }
}
