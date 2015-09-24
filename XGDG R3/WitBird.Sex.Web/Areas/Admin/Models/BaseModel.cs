using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitBird.Sex.Web.Areas.Admin.Models
{
    public class BaseModel
    {
        public string Title { get; set; }

        public string Keywords { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 每页显示页码数
        /// </summary>
        public int PageStep { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int AllCount { get; set; }
    }
}