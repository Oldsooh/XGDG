using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WitBird.Sex.Model;

namespace WitBird.Sex.Web.Areas.Admin.Models
{
    public class VideoModel : BaseModel
    {
        public Video Video { get; set; }

        public List<Video> Videos { get; set; }
    }
}