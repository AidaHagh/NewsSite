using Data.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.SiteVisit
{
    public class VisitService : IVisitService
    {
        private readonly ApplicationDbContext _context;

        public VisitService(ApplicationDbContext context)
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

        //افزایش آمار بازدید  
        //کاربری که ای پی آن در سایت ثبت شده باشد
        public void IncreasVisit(string Ip, string date)
        {
            var result = (from v in _context.Visitors
                          where v.IpAddress.Equals(Ip) && v.DateTime.Equals(date)
                          select v);
            var current = result.FirstOrDefault();

            if (result.Count() != 0)
            {
                current.VisitCount++;


                _context.Visitors.Attach(current);
                _context.Entry(current).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
        }
    }
}
