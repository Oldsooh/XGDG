using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WitBird.Sex.Model;

namespace WitBird.Sex.Web.Models
{
    public class ImageModel
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 该专辑所有图片总数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 该专辑
        /// </summary>
        public Album Album { get; set; }

        /// <summary>
        /// 该专辑所属类别实体
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// 该专辑的图片分页
        /// </summary>
        public List<Image> Images { get; set; }

        /// <summary>
        /// 推荐图册
        /// </summary>
        public List<Album> RecommendAlbum { get; set; }
        /// <summary>
        /// 相关图册
        /// </summary>
        public List<Album> RelatedAlbum { get; set; }

        /// <summary>
        /// 上一图册
        /// </summary>
        public Album LastAlbum { get; set; }
        /// <summary>
        /// 下一图册
        /// </summary>
        public Album NextAlbum { get; set; }
        /// <summary>
        /// 当前图片
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// 上一页
        /// </summary>
        public string Previous { get; set; }

        /// <summary>
        /// 下一页
        /// </summary>
        public string Next { get; set; }

        /// <summary>
        /// 推荐新闻
        /// </summary>
        public List<News> RecommendNews { get; set; }
    }
}