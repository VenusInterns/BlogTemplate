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

        public void OnGet([FromRoute] int id)
        {
            post = _dataStore.GetPost(id);

            if (post == null)
            {
                RedirectToPage("/Index");
            }
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostPublish([FromRoute] int id, [FromForm] bool updateSlug)
        {
            post = _dataStore.GetPost(id);
            post.IsPublic = true;
            UpdatePost(id, updateSlug);
            return Redirect($"/Post/{id}/{editedPost.NewSlug}");
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostSaveDraft([FromRoute] int id, [FromForm] bool updateSlug)
        {
            post = _dataStore.GetPost(id);
            post.IsPublic = false;
            UpdatePost(id, updateSlug);
            return Redirect("/Index");
        }
        private void UpdatePost(int id, [FromForm] bool updateSlug)
        {
            post = _dataStore.GetPost(id);
            post.Title = editedPost.Title;
            post.Body = editedPost.Body;
            post.Excerpt = editedPost.Excerpt;

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
            public string OldSlug { get; }
            public string NewSlug { get; set; }
        }
    }
}
