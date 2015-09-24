using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WitBird.Sex.Model;

namespace WitBird.Sex.Web.Models
{
    public class NewsRightModel : BaseModel
    {
        /// <summary>
        /// 推荐阅读
        /// </summary>
        public List<News> RecommendNews { get; set; }

        /// <summary>
        /// 一周排行
        /// </summary>
        public List<News> TopNews { get; set; }
    }
}