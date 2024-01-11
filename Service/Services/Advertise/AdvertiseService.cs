using Data.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Advertise
{
    public class AdvertiseService : IAdvertiseService
    {
        private readonly ApplicationDbContext _context;

        public AdvertiseService(ApplicationDbContext context)
        {
            _context = context;
        }


        public void changeStatus(int id)
        {
            var result = (from c in _context.Advertisings where c.AdverId == id select c);
            var currentAdvertise = result.FirstOrDefault();

            if (result.Count() != 0)
            {
                if (currentAdvertise.Flag == 0)
                {
                    currentAdvertise.Flag = 1;
                }
                else
                {
                    currentAdvertise.Flag = 0;
                }

                _context.Advertisings.Attach(currentAdvertise);
                _context.Entry(currentAdvertise).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
    }
}
