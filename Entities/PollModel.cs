using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
  
        public class Poll
        {
            public int PollId { get; set; }
            public string Question { get; set; }
            public DateTime PollStartDate { get; set; }
            public DateTime PollEndDate { get; set; }
            public bool Active { get; set; }

            public virtual ICollection<PollOption> pollOption { get; set; }
        }


        public class PollOption
        {
            public int PolloptionID { get; set; }
            public int PollID { get; set; }
            public string Answer { get; set; }
            public int VouteCount { get; set; }

            public virtual Poll poll { get; set; }
        }
   }
