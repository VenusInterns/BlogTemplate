using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTemplate.Models
{
    public class Blog
    {
        public Blog()
        {
            Init();
        }
       
        public List<Post> Posts { get; } = new List<Post>();
        public List<Comment> Comments { get; } = new List<Comment>();
        public object Comment { get; private set; }

        private void Init()
        {

        }
    }
}
