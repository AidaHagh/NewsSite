using Data.DbContext;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Transaction
{
    public class EntityDataBaseTransaction : IEntityDataBaseTransaction
    {
        private readonly IDbContextTransaction _transaction;

        public EntityDataBaseTransaction(ApplicationDbContext context)//یک نمونه از دیتابیس اینیشیال میذاریم-
        {
            _transaction = context.Database.BeginTransaction();
        }


        public void Commit()
        {
            _transaction.Commit();
        }

        public void RollBack()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }


    }
}
