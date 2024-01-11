using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Transaction
{
    public interface IEntityDataBaseTransaction :IDisposable
    {
        void Commit();
        void RollBack();
  



    }
}
