using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WitBird.Sex.Model;

namespace WitBird.Sex.WebV2.Models
{
    public class SearchModel : BaseModel
    {
        public List<Album> AlbumResult { get; set; }

        public List<News> NewsResult { get; set; }

        public List<Album> RecommendAlbum { get; set; }

        public string Keywords { get; set; }

        public int TotalResultsCount { get; set; }

        public int TotalAlbumCount { get; set; }

        public int TotalNewsCount { get; set; }
    }
}