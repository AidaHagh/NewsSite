using Data.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Role
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;
        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            if (_context!=null)
            {
                _context.Dispose();
            }
        }

        public string GetRoleId(string userId)
        {
            var getRoleId = _context.UserRoles.Where(ur => ur.UserId == userId).ToList();

            string getRolIdArray = "";

            for (int i = 0; i < getRoleId.Count; i++)
            {
                getRolIdArray += getRoleId[i].RoleId.ToString() + ",";
            }

            return getRolIdArray;
        }
    }
}
