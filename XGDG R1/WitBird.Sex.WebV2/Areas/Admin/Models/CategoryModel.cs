using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WitBird.Sex.Model;

namespace WitBird.Sex.WebV2.Areas.Admin.Models
{
    public class CategoryModel
    {
        public Category Category { get; set; }

        public List<Category> Categories { get; set; }
    }
}