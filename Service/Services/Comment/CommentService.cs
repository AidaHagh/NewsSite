using Data.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Comment
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
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



        //افزایش تعداد لایک
        public void IncreaseLike(int id)
        {
            var result = (from c in _context.Comments where c.Id == id select c);
            var currentComment = result.FirstOrDefault();

            if (result.Count() != 0)
            {
                currentComment.LikeCount++;
                _context.Comments.Attach(currentComment);
                _context.Entry(currentComment).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
        }

        //افزایش تعداد دیسلایک
        public void IncreasedisLike(int id)
        {
            var result = (from c in _context.Comments where c.Id == id select c);
            var currentComment = result.FirstOrDefault();

            if (result.Count() != 0)
            {
                currentComment.DisLikeCount++;
                _context.Comments.Attach(currentComment);
                _context.Entry(currentComment).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void AcceptOrReject(int id)
        {
            var result = (from c in _context.Comments where c.Id == id select c);
            var currentComment = result.FirstOrDefault();

            if (result.Count() != 0)
            {
                if (currentComment.Status == true)
                {
                    currentComment.Status = false;
                }
                else
                {
                    currentComment.Status = true;
                }

                _context.Comments.Attach(currentComment);
                _context.Entry(currentComment).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
        }






    }
}
