using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WitBird.Sex.Model;

namespace WitBird.Sex.Web.Models
{
    public class VideoModel : BaseModel
    {
        public List<Video> Videos { get; set; }

        public Video Video { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        /// <summary>
        /// 随机推荐
        /// </summary>
        public List<Video> RecommendRandoms { get; set; }

        /// <summary>
        /// 猜你喜欢
        /// </summary>
        public List<Album> Like { get; set; }

        /// <summary>
        /// 推荐新闻
        /// </summary>
        public List<News> RecommendNews { get; set; }

        /// <summary>
        /// 推荐小说
        /// </summary>
        public List<Novel> RecommendNovels { get; set; }

        public Category Category { get; set; }
    }
}