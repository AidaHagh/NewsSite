using AutoMapper;
using Data.ViewModel;
using Entities;
using Entities.news;
using Entities.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping() 
        {
           CreateMap<ApplicationUsers,UsersViewModel>().ReverseMap();    
           CreateMap<ApplicationUsers, EditUserViewModel>().ReverseMap();    
           CreateMap<ApplicationRoles, ApplicationRoleViewModel>().ReverseMap();    
            CreateMap<Category,CategoryViewModel>().ReverseMap();    
            CreateMap<News,NewsViewModel>().ReverseMap();    
            CreateMap<Comment,CommentViewModel>().ReverseMap();    
            CreateMap<Advertising, AdvertisingViewModel>().ReverseMap();    
            CreateMap<NewsPaper, NewsPaperViewModel>().ReverseMap();    

            CreateMap<AboutUs, AboutUsViewModel>().ReverseMap();    
            CreateMap<ContactUs, ContactUsViewModel>().ReverseMap();    
            CreateMap<Social, SocialViewModel>().ReverseMap();    
           
        }

    }
}
