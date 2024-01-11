using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Services.UnitOfWork;

namespace NewsSite.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(int page = 1)
        {

            int paresh = (page - 1) * 5;
            int totalCount = _unitOfWork.visitorUW.Get().DistinctBy(x=>x.IpAddress).Count();
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

            var model = _unitOfWork.visitorUW.Get().Skip(paresh).Take(5);
            return View(model);
        }
    }
}
