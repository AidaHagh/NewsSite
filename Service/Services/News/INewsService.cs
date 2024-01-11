using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.News
{
    public interface INewsService :IDisposable
    {
        void RefreshVisitorCount(int Id);
    }
}
