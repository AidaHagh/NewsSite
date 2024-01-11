using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Advertising
    {
        [Key]
        public int AdverId { get; set; }
        public string GifPath { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Link { get; set; }
        public byte Flag { get; set; }
        public byte AdverLocation { get; set; }
    }
}
