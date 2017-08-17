using System;
using BlogTemplate._1.Models;
using BlogTemplate._1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace BlogTemplate._1.Pages
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
        public EditedPostModel EditedPost { get; set; }

        public void OnGet([FromRoute] int id)
        {
            Post post = _dataStore.GetPost(id);

            EditedPost = new EditedPostModel
            {
                Title = post.Title,
                Body = post.Body,
                Excerpt = post.Excerpt,
            };

            if (post == null)
            {
                RedirectToPage("/Index");
            }
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostPublish([FromRoute] int id, [FromForm] bool updateSlug)
        {
            Post post = _dataStore.GetPost(id);
            post.IsPublic = true;
            UpdatePost(post, updateSlug);
            return Redirect($"/Post/{id}/{post.Slug}");
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostSaveDraft([FromRoute] int id, [FromForm] bool updateSlug)
        {
            Post post = _dataStore.GetPost(id);
            post.IsPublic = false;
            UpdatePost(post, updateSlug);
            return Redirect("/Drafts");
        }
        private void UpdatePost(Post post, [FromForm] bool updateSlug)
        {
            post.Title = EditedPost.Title;
            post.Body = EditedPost.Body;
            post.Excerpt = EditedPost.Excerpt;

            _dataStore.SavePost(post);
            if (updateSlug)
            {
                post.Slug = _slugGenerator.CreateSlug(post.Title);
            }
        }

        public class EditedPostModel
        {
            public string Title { get; set; }
            public string Body { get; set; }
            public string Excerpt { get; set; }
        }
    }
}
