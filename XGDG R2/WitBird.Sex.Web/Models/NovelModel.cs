using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WitBird.Sex.Model;

namespace WitBird.Sex.Web.Models
{
    public class NovelModel : BaseModel
    {
        public NovelCategory NovelCategory { get; set; }

        public Novel Novel { get; set; }

        public List<Novel> Novels { get; set; }

        public NovelArticle NovelArticle { get; set; }

        public List<NovelArticle> NovelArticles { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        /// <summary>
        /// 猜你喜欢
        /// </summary>
        public List<Album> Like { get; set; }

        public Category Category { get; set; }
    }
}