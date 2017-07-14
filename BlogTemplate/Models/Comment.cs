using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTemplate.Models
{
    public class Comment
    {
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public string Body { get; set; }
        public DateTime PubDate { get; set; } = DateTime.Now;
        public bool IsPublic { get; set; }
        public Guid UniqueId { get; set; }
    }
}
