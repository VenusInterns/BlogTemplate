using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BlogTemplate.Models;
using Microsoft.AspNetCore.Authorization;
using BlogTemplate.Services;


namespace BlogTemplate.Pages
{
    [Authorize]
    public class EditModel : PageModel
    {
        private BlogDataStore _dataStore;
        private readonly SlugGenerator _slugGenerator;
        private readonly ExcerptGenerator _excerptGenerator;

        public EditModel(BlogDataStore dataStore, SlugGenerator slugGenerator, ExcerptGenerator excerptGenerator)
        {
            _dataStore = dataStore;
            _slugGenerator = slugGenerator;
            _excerptGenerator = excerptGenerator;
        }

        [BindProperty]
        public EditedPostModel editedPost { get; set; }
        public Post post { get; set; }

        public void OnGet()
        {
            InitializePost();
        }

        private void InitializePost()
        {
            string slug = RouteData.Values["slug"].ToString();
            post = _dataStore.GetPost(slug);

            if (post == null)
            {
                RedirectToPage("/Index");
            }
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostPublish()
        {
            string slug = RouteData.Values["slug"].ToString();
            post.IsPublic = true;
            UpdatePost(post, slug, updateSlug);
            return Redirect($"/Post/{post.Slug}");
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostSaveDraft()
        {
            string slug = RouteData.Values["slug"].ToString();
            post.IsPublic = false;
            UpdatePost(post, slug, updateSlug);
            return Redirect("/Index");
        }

        private void UpdatePost(EditedPostModel editedPost, string slug, [FromForm] bool updateSlug)
        {
            post = _dataStore.GetPost(slug);
            post.Title = editedPost.Title;
            post.Body = editedPost.Body;
            post.Excerpt = editedPost.Excerpt;

            if (updateSlug)
            {
                newPost.Excerpt = _excerptGenerator.CreateExcerpt(newPost.Body, 140);
            }

            _dataStore.SavePost(post);
            if (editedPost.NewSlug != editedPost.OldSlug)
            {
                newPost.Slug = _slugGenerator.CreateSlug(newPost.Title);
            }
        }

        public class EditedPostModel
        {
            public string Title { get; set; }
            public string Body { get; set; }
            public string Excerpt { get; set; }
            public string OldSlug { get; }
            public string NewSlug { get; set; }
        }
    }
}
