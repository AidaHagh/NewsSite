using Data.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Poll
{
    public class PollService : IPollService
    {
        private readonly ApplicationDbContext _context;

        public PollService(ApplicationDbContext context)
        {
            _context = context;
        }



        //بستن نظرسنجی
        public void ClosePoll(int id)
        {
            var result = (from p in _context.Polls where p.PollId == id select p);
            var currentPoll = result.FirstOrDefault();

            if (result.Count() != 0)
            {
                currentPoll.Active = false;
                currentPoll.PollEndDate = DateTime.Now;

                _context.Polls.Attach(currentPoll);
                _context.Entry(currentPoll).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void Dispose()
        {
            if (_context!=null)
            {
                _context.Dispose();
            }
        }

        //ثبت رای 
        public void setVote(int id)
        {
            var result = (from p in _context.PollOptions where p.PolloptionID == id select p);
            var currentPolloption = result.FirstOrDefault();

            if (result.Count() != 0)
            {
                currentPolloption.VouteCount++;


                _context.PollOptions.Attach(currentPolloption);
                _context.Entry(currentPolloption).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
        }


    }
}
