using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BlogTemplate.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace BlogTemplate.Pages
{
    public class PostModel : PageModel
    {
        private Blog _blog;
        private BlogDataStore _dataStore;

        public PostModel(Blog blog, BlogDataStore dataStore)
        {
            _blog = blog;
            _dataStore = dataStore;
        }

        [BindProperty]
        public Comment Comment { get; set; }

        public Post Post { get; set; }

        public void OnGet()
        {
            InitializePost();
        }

        private void InitializePost()
        {
            string slug = RouteData.Values["slug"].ToString();

            Post = _dataStore.GetPost(slug);

            if (Post == null)
            {
                RedirectToPage("/Index");
            }
        }

        public IActionResult OnPostPublishComment()
        {
            string slug = RouteData.Values["slug"].ToString();

            Post = _dataStore.GetPost(slug);

            if (Post == null)
            {
                RedirectToPage("/Index");
            }
            else if (ModelState.IsValid)
            {
                Comment.IsPublic = true;
                Comment.UniqueId = Guid.NewGuid();
                Post.Comments.Add(Comment);
                _dataStore.SavePost(Post);
            }
            return Page();
        }

        public IActionResult OnPostDeleteComment(Guid commentId)
        {
            string slug = RouteData.Values["slug"].ToString();
            Post = _dataStore.GetPost(slug);

            Comment foundComment = _dataStore.FindComment(commentId, Post);
            foundComment.IsPublic = false;

            _dataStore.SavePost(Post);
            return Page();
        }

        public IActionResult OnPostUndeleteComment(Guid commentId)
        {
            string slug = RouteData.Values["slug"].ToString();
            Post = _dataStore.GetPost(slug);

            Comment foundComment = _dataStore.FindComment(commentId, Post);
            foundComment.IsPublic = true;

            _dataStore.SavePost(Post);
            return Page();
        }
    }
}
