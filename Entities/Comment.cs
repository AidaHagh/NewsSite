using Entities.news;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public int NewsId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public string IP { get; set; }

        public DateTime CommentDate { get; set; }

        public DateTime CommentTime { get; set; }

        public int LikeCount { get; set; }
        public int DisLikeCount { get; set; }
        public bool Status { get; set; }

        public int ReplyID { get; set; }


        [ForeignKey("NewsId")]
        public virtual News News { get; set; }
    }
}
