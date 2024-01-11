using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ApplicationUsers:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte gender { get; set; }
        public override string PhoneNumber { get; set; }
        public string ImagePath { get; set; }
        public DateTime BirthDayDate { get; set; }
        public bool IsActive { get; set; }
    }
}
