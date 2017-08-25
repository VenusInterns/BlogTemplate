using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace BlogTemplate._1.Models
{
    public class Post
    {
        public int Id { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        [Required(ErrorMessage = "Title required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Post text required")]
        public string Body { get; set; }
        public DateTimeOffset PubDate { get; set; }
        public DateTimeOffset LastModified { get; set; } = DateTimeOffset.Now;
        public string Slug{ get; set; }
        public bool IsPublic { get; set; }
        public string Excerpt { get; set; }
        public int ExcerptMaxLength { get; } = 140;
    }
}
