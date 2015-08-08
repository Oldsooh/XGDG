using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitBird.Sex.Model
{
    public class NovelCategory
    {
        /// <summary>
        /// 小说分类Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
    }
}
