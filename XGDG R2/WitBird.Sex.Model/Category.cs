using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitBird.Sex.Model
{
    public class Category
    {
        public string Id { get; set; }
        public string EntityType { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int OrderNumber { get; set; }
        public bool IsShow { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
