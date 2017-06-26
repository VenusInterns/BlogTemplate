using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BlogTemplate.Models
{
    public class Post
    {
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime PubDate { get; set; } = DateTime.Now;
        public DateTime LastModified { get; set; } = DateTime.Now;
        public List<string> Tags { get; set; } = new List<string>();
        public string Slug{ get; set; }
        public bool IsPublic { get; set; }
        public string Excerpt { get; set; }

    }
}
