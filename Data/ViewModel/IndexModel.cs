
using Entities;
using Entities.news;
using Entities.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ViewModel
{
    public class IndexModel
    {
        public List<News> SliderNews { get; set; }
        public List<News> SpecialNews { get; set; }
        public List<News> LastVideo { get; set; }   
        
        public List<News> LastNews { get; set; }
        public List<News> InternalNews { get; set; }
        public List<News> ForeignNews { get; set; }
        public List<News> privateNews { get; set; }

        public List<NewsPaper> GetNewsPapers { get; set; }

        public News NewsDetail { get; set; }

        public List<Advertising> Adver { get; set; }
        public List<News> searchmodel { get; set; }
        public Poll PollModel { get; set; }

        public AboutUs aboutUs { get; set; }
        public ContactUs contactUs { get; set; }
        public List<Social> social { get; set; }

        public LoginViewModel LoginVM { get; set; }
    }
}
