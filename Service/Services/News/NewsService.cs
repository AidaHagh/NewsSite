using Data.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.News
{
    public class NewsService : INewsService
    {
        private readonly ApplicationDbContext _context;

        public NewsService(ApplicationDbContext context)
        {
            _context = context;
        }


        public void Dispose()
        {
           if (_context != null)
            {
                _context.Dispose();
            }
        }


        //به روز رسانی تعداد بازدید
        public void RefreshVisitorCount(int Id)
        {
            var result = (from n in _context.NewsTbl where n.NewsId == Id select n);//دستورات لینک
            var currentNews = result.FirstOrDefault();

            if (result.Count() != 0)
            {
                currentNews.VisitCount++;
                _context.NewsTbl.Attach(currentNews);
                _context.Entry(currentNews).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
        }
    }
}
