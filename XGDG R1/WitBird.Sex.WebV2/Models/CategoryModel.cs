using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WitBird.Sex.Model;

namespace WitBird.Sex.WebV2.Models
{
    public class CategoryModel : BaseModel
    {
        /// <summary>
        /// 该类别实体
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// 该类别的专辑分页
        /// </summary>
        public List<Album> Albums { get; set; }
    }
}