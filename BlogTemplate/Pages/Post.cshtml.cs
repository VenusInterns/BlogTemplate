using System;
using System.ComponentModel.DataAnnotations;
using BlogTemplate._1.Models;
using BlogTemplate._1.Services;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogTemplate._1.Pages
{

    public class PostModel : PageModel
    {
        private readonly BlogDataStore _dataStore;
        private readonly MarkdownRenderer _markdownRenderer;
        const int MaxAllowedComments = 100;

        public PostModel(BlogDataStore dataStore, MarkdownRenderer markdownRenderer)
        {
            _dataStore = dataStore;
            _markdownRenderer = markdownRenderer;
        }

        public Post Post { get; set; }
        public bool IsCommentsFull => Post.Comments.Count >= MaxAllowedComments;

        public HtmlString HtmlBody()
        {
            var html = _markdownRenderer.RenderMarkdown(Post.Body);
            return html;
        }

        public void OnGet([FromRoute] string id)
        {
            Post = _dataStore.GetPost(id);

            if (Post == null || !Post.IsPublic)
            {
                RedirectToPage("/Index");
            }
        }

        public IActionResult ChangeState(bool Deleted, string id)
        {
            Post = _dataStore.GetPost(id);

            if (ModelState.IsValid)
            {
                Post.IsDeleted = Deleted;
                _dataStore.SavePost(Post);
                return RedirectToPage("/Index");
            }
            return Redirect("/post/" + id + "/" + Post.Slug);
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostDeletePost([FromRoute] string id)
        {
            return ChangeState(true, id);
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostUnDeletePost([FromRoute] string id)
        {
            return ChangeState(false, id);
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostPublishComment([FromRoute] string id, [FromForm] CommentViewModel NewComment)
        {
            Post = _dataStore.GetPost(id);

            if (Post == null)
            {
                return RedirectToPage("/Index");
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
