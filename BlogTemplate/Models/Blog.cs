using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTemplate.Models
{
    public class Blog
    {
        public int CurrentId { get; set; } = 0;
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<Comment> Comments { get; } = new List<Comment>();
        public object Comment { get; private set; }
    }
}
