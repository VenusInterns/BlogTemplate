
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

        [BindProperty]
        public CommentViewModel NewComment { get; set; }
        public bool IsCommentsFull => Post.Comments.Count >= MaxAllowedComments;
 

        public Post Post { get; set; }

        public void OnGet([FromRoute] int id)
        {
            Post = _dataStore.GetPost(id);

            if (Post == null)
            {
                RedirectToPage("/Index");
            }
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
                    AuthorName = NewComment.AuthorName,
                    Body = NewComment.Body,
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
            [MaxLength(100, ErrorMessage = "You have exceeded the maximum length of 100 characters")]
            public string AuthorName { get; set; }

            [Required]
            [MaxLength(1000, ErrorMessage = "You have exceeded the maximum length of 1000 characters")]
            public string Body { get; set; }
        }
    }
}
