using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitBird.Sex.Model
{
    public class HomeData
    {
        /// <summary>
        /// 幻灯片
        /// </summary>
        public List<Slide> Slides { get; set; }
        /// <summary>
        /// 最新资讯
        /// </summary>
        public List<News> NewNews { get; set; }
        /// <summary>
        /// 最新上传
        /// </summary>
        public List<Album> NewUploads { get; set; }
        /// <summary>
        /// 推荐资讯
        /// </summary>
        public List<News> RecommendNews { get; set; }
        /// <summary>
        /// 最新推荐
        /// </summary>
        public List<Album> RecommendPictures { get; set; }
        /// <summary>
        /// 今日最美
        /// </summary>
        public List<Album> TopWomen { get; set; }
        /// <summary>
        /// 今日最帅
        /// </summary>
        public List<Album> TopMen { get; set; }
        /// <summary>
        /// 推荐青纯
        /// </summary>
        public List<Album> RecommendPure { get; set; }
        /// <summary>
        /// 推荐性感
        /// </summary>
        public List<Album> RecommendSexy { get; set; }
        /// <summary>
        /// 推荐明星
        /// </summary>
        public List<Album> RecommendStar { get; set; }
        /// <summary>
        /// 推荐古装
        /// </summary>
        public List<Album> RecommendCostume { get; set; }
        /// <summary>
        /// 推荐型男
        /// </summary>
        public List<Album> RecommendStylishMan { get; set; }
    }
}
