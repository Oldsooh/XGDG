using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WitBird.Sex.Model;

namespace WitBird.Sex.Web.Areas.Admin.Models
{
    public class NewsModel : BaseModel
    {
        public News News { get; set; }

        public List<News> NewsList { get; set; }

        public NewsCategory NewsCategory { get; set; }

        public List<NewsCategory> NewsCategories { get; set; }
    }
}