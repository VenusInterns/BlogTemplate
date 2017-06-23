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

        private void Init()
        {
            var post1 = new Post
            {
                Title = "My new post",
                Slug = "my-new-post",
                Excerpt = "My new excerpt",
                Body = "The body of my new post",
                IsPublic = true,
                Tags = new List<string> { "tag1", "tag2" },
            };

            var post2 = new Post
            {
                Title = "Something interesting",
                Slug = "something-interesting",
                Excerpt = "My new excerpt",
                Body = "The body of my new post",
                IsPublic = true,
                Tags = new List<string> { "tag1" },
            };

            Posts.Add(post1);
            Posts.Add(post2);
        }
    }
}
