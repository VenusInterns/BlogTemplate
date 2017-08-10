using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace BlogTemplate.Models
{
    public class Post
    {
        public int Id { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        [Required(ErrorMessage = "Title required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Post text required")]
        public string Body { get; set; }
        public DateTime PubDate { get; set; }
        public DateTime LastModified { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public string Slug{ get; set; }
        public bool IsPublic { get; set; }
        public string Excerpt { get; set; }

    }
}
