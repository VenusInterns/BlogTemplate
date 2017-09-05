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

        public PostModel(BlogDataStore dataStore, MarkdownRenderer markdownRenderer)
        {
            _dataStore = dataStore;
            _markdownRenderer = markdownRenderer;
        }

        public Post Post { get; set; }

        public HtmlString HtmlBody()
        {
            var html = _markdownRenderer.RenderMarkdown(Post.Body);
            return html;
        }

        public IActionResult OnGet([FromRoute] int id)
        {
            Post = _dataStore.GetPost(id);

            if (Post == null || !Post.IsPublic)
            {
                return RedirectToPage("/Index");
            }


            CommentViewModel NewComment = new CommentViewModel();
            return Page();
        }

        public IActionResult ChangeState(bool Deleted, int id)
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
        public IActionResult OnPostDeletePost([FromRoute] int id)
        {
            return ChangeState(true, id);
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostUnDeletePost([FromRoute] int id)
        {
            return ChangeState(false, id);
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostPublishComment([FromRoute] int id, [FromForm] CommentViewModel NewComment)
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
            public string AuthorName { get; set; }
            [Required]
            public string Body { get; set; }
        }
    }
}
