using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitBird.Sex.Model
{
    public class Novel
    {
        public int Id { get; set; }
        public string CategoryId { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int ArticleCount { get; set; }
        public DateTime InsertTime { get; set; }
        public int ViewTime { get; set; }
        public bool IsDelete { get; set; }

        public NovelCategory Category { get; set; }
    }
}
