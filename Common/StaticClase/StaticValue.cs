using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.StaticClase;

public static class StaticValue
{

    //جستجو در کاربران
    public class UserSearchTypeModel
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public List<UserSearchTypeModel> GetUserTypeModel()
        {
            var model = new List<UserSearchTypeModel>
            {
                new UserSearchTypeModel {ID = 0, Title = "---انتخاب---"},
                new UserSearchTypeModel {ID = 1, Title = "نام"},
                new UserSearchTypeModel {ID = 2, Title = "نام خانوادگی"},
                new UserSearchTypeModel {ID = 3, Title = "شماره تلفن"},
                new UserSearchTypeModel {ID = 4, Title = "ایمیل"},
                new UserSearchTypeModel {ID = 5, Title = "نام کاربری"},

            };
            return model;
        }
    }

    // جستجو در نظرات
    public class CommentSearchTypeModel
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public List<CommentSearchTypeModel> GetCommentSearch()
        {
            var _model = new List<CommentSearchTypeModel>
            {
                new CommentSearchTypeModel {ID=0,Title="---انتخاب---"},
                new CommentSearchTypeModel {ID=1,Title="نام و نام خانوادگی"},
                new CommentSearchTypeModel {ID=2,Title="ایمیل"},
                new CommentSearchTypeModel {ID=3,Title="IP"},
                new CommentSearchTypeModel {ID=4,Title="تاریخ ثبت"},
                new CommentSearchTypeModel {ID=5,Title="ساعت ثبت"},
                new CommentSearchTypeModel {ID=6,Title="عنوان خبر"},
                new CommentSearchTypeModel {ID=7,Title="وضعیت"},
                new CommentSearchTypeModel {ID=8,Title="متن پیام"},
            };

            return _model;
        }
    }



    //محل خبر
    public class IndexPageSection
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public List<IndexPageSection> GetPalceIndex()
        {
            var model = new List<IndexPageSection>
            {
                new IndexPageSection {ID = 1, Title = "اسلایدر"},
                new IndexPageSection {ID = 2, Title = "اخبار ویژه"},
                new IndexPageSection {ID = 3, Title = "آخرین مطالب"},
                new IndexPageSection {ID = 4, Title = "آخرین ویدیوها"},
            };
            return model;
        }
    }



    //(آخرین خبر (داخلی خارجی اختصاصی 
    public class TypeNewsModel
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public List<TypeNewsModel> GetTypeNews()
        {
            var model = new List<TypeNewsModel>
            {
                new TypeNewsModel {ID = 1, Title = "داخلی"},
                new TypeNewsModel {ID = 2, Title = "خارجی"},
                new TypeNewsModel {ID = 3, Title = "اختصاصی"},
            };
            return model;
        }
    }



    //محل نمایش تبلیغات
    public class AdvertisePlace
    {
        public int AdvId { get; set; }
        public string AdvLocationName { get; set; }

        public List<AdvertisePlace> AdvertiseDescription()
        {
            var model = new List<AdvertisePlace>
            {
                new AdvertisePlace {AdvId = 1, AdvLocationName = "هدر سایت"},
                new AdvertisePlace {AdvId = 2, AdvLocationName = "زیر اسلایدر"},
                new AdvertisePlace {AdvId = 3, AdvLocationName = "زیر اخبار ویژه"},
                new AdvertisePlace {AdvId = 4, AdvLocationName = "زیر آخرین ویدیوها"},
                new AdvertisePlace {AdvId = 5, AdvLocationName = "سمت چپ"},
            };
            return model;
        }
    }

}

