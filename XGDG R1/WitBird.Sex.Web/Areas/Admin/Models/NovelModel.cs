using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WitBird.Sex.Model;

namespace WitBird.Sex.Web.Areas.Admin.Models
{
    public class NovelModel : BaseModel
    {
        public Novel Novel { get; set; }

        public List<Novel> Novels { get; set; }

        public NovelCategory NovelCategory { get; set; }

        public List<NovelCategory> NovelCategories { get; set; }

        public NovelArticle NovelArticle { get; set; }

        public List<NovelArticle> NovelArticles { get; set; }
    }
}