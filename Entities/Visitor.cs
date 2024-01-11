using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Visitor
    {
        [Key]
        public int Id { get; set; }

        public string IpAddress { get; set; }

        public DateTime DateTime { get; set; }

        public int VisitCount { get; set; }
    }
}
