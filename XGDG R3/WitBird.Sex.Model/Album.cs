using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitBird.Sex.Model
{
    public class Album
    { 
        public int Id { get; set; }
        public string CategoryId { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public bool IsDelete { get; set; }
        public bool IsShow { get; set; }
        public bool IsTop { get; set; }
        public DateTime InsertTime { get; set; }
        public int ViewTime { get; set; }
        public int ImagesCount { get; set; }
        public string UrlOriginal { get; set; }
        public string UrlThumbnailWidth102x102 { get; set; }
        public string UrlThumbnailWidth235x350 { get; set; }
        public string UrlThumbnailWidth490x350 { get; set; }
        public string UrlThumbnailHeight200 { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
