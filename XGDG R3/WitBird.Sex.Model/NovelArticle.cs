using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitBird.Sex.Model
{
    public class NovelArticle
    {
        public int Id { get; set; }

        public int NovelId { get; set; }

        public string Title { get; set; }

        public string ContentStyle { get; set; }

        public DateTime InsertTime { get; set; }
             
    }
}
