using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitBird.Sex.Model
{
    public class News
    {
        /// <summary>
        /// 新闻ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 新闻分类ID
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 图组
        /// </summary>
        public string PictureGroup { get; set; }

        /// <summary>
        /// 新闻内容（带格式）
        /// </summary>
        public string ContentStyle { get; set; }

        /// <summary>
        /// 新闻内容（不带格式）
        /// </summary>
        public string ContentText { get; set; }

        /// <summary>
        /// 发布类型
        /// </summary>
        public string PostType { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string ComeFrom { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime InsertTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 访问次数
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool IsRecommend { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsTop { get; set; }

        /// <summary>
        /// 是否幻灯
        /// </summary>
        public bool IsSlide { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public NewsCategory Category { get; set; }
    }
}
