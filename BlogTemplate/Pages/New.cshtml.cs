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
    public class NewModel : PageModel
    {

        const string StorageFolder = "BlogFiles";

        private readonly BlogDataStore _dataStore;

        private readonly SlugGenerator _slugGenerator;
        private readonly ExcerptGenerator _excerptGenerator;


        public NewModel(BlogDataStore dataStore, SlugGenerator slugGenerator, ExcerptGenerator excerptGenerator)
        {
            _dataStore = dataStore;
            _slugGenerator = slugGenerator;
            _excerptGenerator = excerptGenerator;
        }
        public void OnGet()
        {
        }

        public Post Post { get; set; }

        [BindProperty]
        public NewViewModel NewPost { get; set; }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostPublish()
        {
            if (ModelState.IsValid)
            {
                Post.PubDate = DateTime.UtcNow;
                Post.LastModified = DateTime.UtcNow;
                Post.IsPublic = true;

                SavePost(Post);
                return Redirect("/Index");
            }

            return Page();
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostSaveDraft()
        {
            if(ModelState.IsValid)
            {
                Post.IsPublic = false;

                SavePost(Post);
                return Redirect("/Index");
            }

            return Page();
        }

        private void SavePost(Post post)
        {
            Post.Title = NewPost.Title;
            Post.Body = NewPost.Body;
            Post.Excerpt = NewPost.Excerpt;
            Post.Slug = _slugGenerator.CreateSlug(Post.Title);

            if (string.IsNullOrEmpty(Post.Excerpt))
            {
                Post.Excerpt = _excerptGenerator.CreateExcerpt(Post.Body, 140);
            }

            _dataStore.SavePost(Post);
        }

        public class NewViewModel
        {
            public string Title { get; set; }
            public string Body { get; set; }
            public string Excerpt { get; set; }
        }
    }
}
