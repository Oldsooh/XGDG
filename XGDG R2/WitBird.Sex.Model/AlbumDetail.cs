using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitBird.Sex.Model
{
    public class AlbumDetail
    {
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
    }
}
