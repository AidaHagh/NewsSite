using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.SiteVisit
{
    public interface IVisitService :IDisposable
    {
        void IncreasVisit(string Ip, string date);
    }
}
