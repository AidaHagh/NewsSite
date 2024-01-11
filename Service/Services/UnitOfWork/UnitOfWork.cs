using Data.DbContext;
using Entities;
using Entities.news;
using Entities.Setting;
using Service.Repository;
using Service.Services.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context) 
        { 
            _context = context; 
        }

        private GenericRepository<Category> _category;

        private GenericRepository<Entities.news.News> _news;

        private GenericRepository<ApplicationUsers> _user;

        private GenericRepository<Entities.Comment> _comment;

        private GenericRepository<Entities.Advertising> _advertising;

        private GenericRepository<Entities.Poll> _poll;

        private GenericRepository<Entities.PollOption> _pollOption;

        private GenericRepository<Entities.Visitor> _visitor;

        private GenericRepository<Entities.news.NewsPaper> _newsPaper;

        private GenericRepository<Entities.Setting.AboutUs> _aboutUs ;
        private GenericRepository<Entities.Setting.ContactUs> _contactUs;
        private GenericRepository<Entities.Setting.Social> _social;






        //دسته بندی اخبار
        public GenericRepository<Category> categoryUW 
        {
            //فقط خواندنی
            get
            {
                if (_category==null)
                {
                    _category = new GenericRepository<Category>(_context);
                }
                return _category;
            } 
        }

        // اخبار
        public GenericRepository<Entities.news.News> newsUW
        {
            //فقط خواندنی
            get
            {
                if (_news == null)
                {
                    _news = new GenericRepository<Entities.news.News>(_context);
                }

                return _news;
            }
        }

        //کاربران
        public GenericRepository<ApplicationUsers> userUW
        {
            //فقط خواندنی
            get
            {
                if (_user == null)
                {
                    _user = new GenericRepository<ApplicationUsers>(_context);
                }
                return _user;
            }
        }

        // نظرات
        public GenericRepository<Entities.Comment> commentUW
        {
            //فقط خواندنی
            get
            {
                if (_comment == null)
                {
                    _comment = new GenericRepository<Entities.Comment>(_context);
                }
                return _comment;
            }
        }    

        // تبلیغات
        public GenericRepository<Entities.Advertising> adverUW
        {
            //فقط خواندنی
            get
            {
                if (_advertising == null)
                {
                    _advertising = new GenericRepository<Entities.Advertising>(_context);
                }
                return _advertising;
            }
        }


        // نظرسنجی
        public GenericRepository<Entities.Poll> pollUW
        {
            //فقط خواندنی
            get
            {
                if (_poll == null)
                {
                    _poll = new GenericRepository<Entities.Poll>(_context);
                }
                return _poll;
            }
        }

        // پاسخ نظرسنجی
        public GenericRepository<Entities.PollOption> pollOptionUW
        {
            //فقط خواندنی
            get
            {
                if (_pollOption == null)
                {
                    _pollOption = new GenericRepository<Entities.PollOption>(_context);
                }
                return _pollOption;
            }
        }

        //مدیریت تراکنش
        public IEntityDataBaseTransaction BeginTransac()
        {
            return new EntityDataBaseTransaction(_context);
        }

        //بازدید کننده
        public GenericRepository<Visitor> visitorUW
        {
            //فقط خواندنی
            get
            {
                if (_visitor == null)
                {
                    _visitor = new GenericRepository<Visitor>(_context);
                }
                return _visitor;
            }
        }

        // درباره ما
        public GenericRepository<AboutUs> aboutUsUW
        {
            //فقط خواندنی
            get
            {
                if (_aboutUs == null)
                {
                    _aboutUs = new GenericRepository<AboutUs>(_context);
                }
                return _aboutUs;
            }
        }

        // ارتباط با ما
        public GenericRepository<ContactUs> contactUsUW
        {
            //فقط خواندنی
            get
            {
                if (_contactUs == null)
                {
                    _contactUs = new GenericRepository<ContactUs>(_context);
                }
                return _contactUs;
            }
        }     
        

        
        // شبکه های اجتماعی
        public GenericRepository<Social> socialUW
        {
            //فقط خواندنی
            get
            {
                if (_social == null)
                {
                    _social = new GenericRepository<Social>(_context);
                }
                return _social;
            }
        }  

        // روزنامه
        public GenericRepository<NewsPaper> newsPaperUW
        {
            //فقط خواندنی
            get
            {
                if (_newsPaper == null)
                {
                    _newsPaper = new GenericRepository<NewsPaper>(_context);
                }
                return _newsPaper;
            }
        }






        public void Dispose()
        {
            if (_context!=null)
            {
                _context.Dispose();
            }
        }

        public void save()
        {
            _context.SaveChanges();
        }
    }
}
