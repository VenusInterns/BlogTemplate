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

            var post1 = new Post
            {
                Title = "My new post",
                Slug = "my-new-post",
                Excerpt = "My new excerpt",
                Body = "The body of my new post",
                IsPublic = true,
                Tags = new List<string> { "tag1", "tag2" },
            };


            var comment1 = new Comment
            {
                AuthorName = "Will",
                AuthorEmail = "carol@microsoft.com",
                Body = "Hello this is the body",
                IsPublic = true,
            };
            post1.Comments.Add(comment1);







            var post2 = new Post
            {
                Title = "Something interesting",
                Slug = "something-interesting",
                Excerpt = "My new excerpt",
                Body = "The body of my new post",
                IsPublic = true,
                Tags = new List<string> { "tag1" },
            };


            var comment2 = new Comment
            {
                AuthorName = "Joe",
                AuthorEmail = "john@microsoft.com",
                Body = "This is comment number 2",
                IsPublic = true,
            };
            post2.Comments.Add(comment2);

            var comment3 = new Comment
            {
                AuthorName = "Tom",
                AuthorEmail = "tom@microsoft.com",
                Body = "This is the third comment.",
                IsPublic = true,
            };
            post2.Comments.Add(comment3);


            Posts.Add(post1);
            Posts.Add(post2);
        }
    }
}
