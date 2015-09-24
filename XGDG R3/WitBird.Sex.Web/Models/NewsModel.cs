using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WitBird.Sex.Model;

namespace WitBird.Sex.Web.Models
{
    public class NewsModel : BaseModel
    {
        public List<News> NewsList { get; set; }

        public News News { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public Category Category { get; set; }

        /// <summary>
        /// 相关图集
        /// </summary>
        public List<Album> RelatedAlbum { get; set; }

        /// <summary>
        /// 相关阅读
        /// </summary>
        public List<News> RelatedNews { get; set; }

        public News LastNews { get; set; }

        public News NextNews { get; set; }
    }
}