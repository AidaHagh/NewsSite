using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Comment
{
    public interface ICommentService : IDisposable
    {
        void IncreaseLike(int id);
        void IncreasedisLike(int id);
        void AcceptOrReject(int id);
    }
}
