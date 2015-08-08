using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitBird.Sex.Model
{
    public class Image
    {
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public string UrlOriginal { get; set; }
        public string UrlThumbnailWidth102x102 { get; set; }
        public string UrlThumbnailWidth235x350 { get; set; }
        public string UrlThumbnailWidth490x350 { get; set; }
        public string UrlThumbnailHeight200 { get; set; }
    }
}
