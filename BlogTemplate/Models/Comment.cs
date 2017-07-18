using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTemplate.Models
{
    public class Comment
    {
        [Required(ErrorMessage = "Name required")]
        public string AuthorName { get; set; }
        [Required(ErrorMessage = "Email required")]
        [EmailAddress]
        public string AuthorEmail { get; set; }
        [Required(ErrorMessage = "Comment text required")]
        public string Body { get; set; }
        public DateTime PubDate { get; set; } = DateTime.Now;
        public bool IsPublic { get; set; }
        public Guid UniqueId { get; set; }
    }
}
