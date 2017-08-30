using System;
using System.ComponentModel.DataAnnotations;
using BlogTemplate._1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogTemplate._1.Pages
{

    public class PostModel : PageModel
    {
        private readonly BlogDataStore _dataStore;
        public PostModel(BlogDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        const int MaxAllowedComments = 100;
        const int MaxNameLength = 100;
        const int MaxBodyLength = 1000;

        [BindProperty]
        public CommentViewModel NewComment { get; set; }
        public bool IsCommentsFull { get { return Post.Comments.Count >= MaxAllowedComments; } }
        public Post Post { get; set; }

        public void OnGet([FromRoute] int id)
        {
            Post = _dataStore.GetPost(id);

            if (Post == null)
            {
                RedirectToPage("/Index");
            }
        }

        public string Trim(string body, int maxLength)
        {
            string TrimmedBody = body;
            if(body.Length >= maxLength)
            {
                TrimmedBody = body.Substring(0, maxLength);
            }
            return TrimmedBody;
        }
        [ValidateAntiForgeryToken]
        public IActionResult OnPostPublishComment([FromRoute] int id)
        {
            Post = _dataStore.GetPost(id);

            if (Post == null)
            {
                RedirectToPage("/Index");
            }
            else if (ModelState.IsValid)
            {
                Comment comment = new Comment
                {
                    AuthorName = Trim(NewComment.AuthorName, MaxNameLength),
                    Body = Trim(NewComment.Body, MaxBodyLength),
                };
                comment.IsPublic = true;
                comment.UniqueId = Guid.NewGuid();
                Post.Comments.Add(comment);
                _dataStore.SavePost(Post);
                return Redirect("/post/" + id + "/" + Post.Slug);
            }

            return Page();
        }

        public class CommentViewModel
        {
            [Required]
            public string AuthorName { get; set; }
            [Required]
            public string Body { get; set; }
        }
    }
}
