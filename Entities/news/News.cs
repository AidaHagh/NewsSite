using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.news
{
    public class News
    {
        [Key]
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Abstract { get; set; }
        public int VisitCount { get; set; }
        public DateTime NewsDate { get; set; }
        public DateTime NewsTime { get; set; }
        public string IndexImage { get; set; }
        public byte PlaceNewsId { get; set; }
        public byte NewsType { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }

        //SEO Property
        public string MetaTag { get; set; }
        public string MetaDescription { get; set; }


  


        [ForeignKey("UserId")]
        public virtual ApplicationUsers User { get; set; }


        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }



    }
}
