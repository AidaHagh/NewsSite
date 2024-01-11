using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Advertise
{
    public interface IAdvertiseService : IDisposable
    {
        void changeStatus(int id);
    }
}
