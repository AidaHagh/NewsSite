using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.news
{
    public class NewsPaper
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int VisitCount { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string UserId { get; set; }

        //SEO Property
        public string MetaTag { get; set; }
        public string MetaDescription { get; set; }



        [ForeignKey("UserId")]
        public virtual ApplicationUsers User { get; set; }
    }
}
