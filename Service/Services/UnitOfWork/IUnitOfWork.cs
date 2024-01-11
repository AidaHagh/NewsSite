using Entities;
using Entities.news;
using Service.Repository;
using Service.Services.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        GenericRepository<Category> categoryUW { get; }
        GenericRepository<Entities.news.News> newsUW { get; }
        GenericRepository<ApplicationUsers> userUW { get; }
        GenericRepository<Entities.Comment> commentUW { get; }
        GenericRepository<Entities.Advertising> adverUW { get; }
        GenericRepository<Entities.Poll> pollUW { get; }
        GenericRepository<Entities.PollOption> pollOptionUW { get; }
        GenericRepository<Entities.Visitor> visitorUW { get; }

        GenericRepository<Entities.Setting.AboutUs> aboutUsUW { get; }
        GenericRepository<Entities.Setting.ContactUs> contactUsUW { get; }
        GenericRepository<Entities.Setting.Social> socialUW { get; }
        GenericRepository<NewsPaper> newsPaperUW { get; }

        IEntityDataBaseTransaction BeginTransac();
        void save();
        
    }
}
