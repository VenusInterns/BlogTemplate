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
        public List<Comment> Comments { get; set; } = new List<Comment>();
        [Required(ErrorMessage = "Title required")]
        [MinLength(1)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Post text required")]
        [MinLength(1)]
        public string Body { get; set; }
        public DateTime PubDate { get; set; } = DateTime.Now;
        public DateTime LastModified { get; set; } = DateTime.Now;
        public string TagsString { get; set; }
        // Add a custom model binder
        //[ModelBinder(BinderType = typeof(TagsEntityBinder))]
        public List<string> Tags { get; set; } = new List<string>();
        public string Slug{ get; set; }
        public bool IsPublic { get; set; }
        public string Excerpt { get; set; }

    }
}
