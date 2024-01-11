using Data.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repository
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<TEntity> _table;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _table=context.Set<TEntity>();  //مقداری که حین اجرا فرستاده میشود--مثل جدول یوزر
        }

        public virtual void Create(TEntity entity)//هر جا خواستیم اطلاعاتی ثبت کنیم و ورودی هم نام جدول می نویسیم
        {
            _table.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _table.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual TEntity GetById(object id)//یک تابع از نوع آن جدول که قبل از دیلیت آنرا پیدا می کند
        {
            return _table.Find(id);//رکورد را اینجا پیدا کردیم
        }

        public virtual void Delete(TEntity entity)//گاهی رکورد ایجاد میشود و قبل از اینکه سیو شود در دیتابیس میخواهد حذف هم بشود
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _table.Attach(entity);
            }
            _table.Remove(entity);
        }

        public virtual void DeleteById(object id)//بعد از یافتن ای دی آنرا حذف میکند
        {
            var entity = GetById(id);
            Delete(entity);
        }

        public virtual void DeleteByRange(Expression<Func<TEntity, bool>> whereVariable = null)
        {
            IQueryable<TEntity> query = _table;
            if (whereVariable != null)
            {
                query = query.Where(whereVariable);
            }
            _table.RemoveRange(query);
        }

        //گرفتن کل اطلاعات جدول 
        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> whereVariable = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderbyVariable = null,
            string joinString = "")
        {
            IQueryable<TEntity> query = _table;

            if (whereVariable != null)
            {
                query = query.Where(whereVariable);
            }
            if (orderbyVariable != null)
            {
                query = orderbyVariable(query);
            }
            if (joinString != "")
            {
                foreach (string item in joinString.Split(','))
                {
                    query = query.Include(item);//هر جدول با یک ویلگول با جدول دیگر جد میشود -- 
                                                //includ کار join را انجام میدهد
                }
            }
            return query.ToList();
        }
    }
}
