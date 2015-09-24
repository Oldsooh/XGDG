using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WitBird.Sex.Model;

namespace WitBird.Sex.Web.Areas.T.Models
{
    public class ImagesModel
    {
        public Album Album { get; set; }

        public List<Album> AlbumList { get; set; }

        public Category Category { get; set; }

        public List<Category> Categories { get; set; }

        public string Keywords { get; set; }
    }
}