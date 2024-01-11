using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Role
{
    public interface IRoleService : IDisposable
    {
        string GetRoleId(string userId);
    }
}
