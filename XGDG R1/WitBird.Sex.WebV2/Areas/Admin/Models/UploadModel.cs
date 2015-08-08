using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WitBird.Sex.Model;

namespace WitBird.Sex.WebV2.Areas.Admin.Models
{
    public class UploadModel
    {
        public List<Category> CategoryList { get; set; }

        public string isSuccessful { get; set; }

        public string SelectedCategory { get; set; }
    }
}