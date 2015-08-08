using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitBird.Sex.Model
{
    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string Url { get; set; }
        public string UrlM { get; set; }
        public DateTime InsertTime { get; set; }
        public int ViewTime { get; set; }
        public bool IsDelete { get; set; }
    }
}
