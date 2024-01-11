using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Poll
{
    public interface IPollService :IDisposable
    {
        void ClosePoll(int id);
        void setVote(int id);
    }
}
