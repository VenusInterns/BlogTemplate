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

        [BindProperty]
        public CommentViewModel NewComment { get; set; }

        public Post Post { get; set; }

        public HtmlString HtmlBody()
        {
            var html = _markdownRenderer.RenderMarkdownToHtml(Post.Body);
            return html;
        }

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
            public string AuthorName { get; set; }
            [Required]
            public string Body { get; set; }
        }
    }
}
