using System;
using BlogTemplate._1.Models;
using BlogTemplate._1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace BlogTemplate._1.Pages
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

        [BindProperty]
        public Post Post { get; set; }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostPublish()
        {
            if (ModelState.IsValid)
            {
                Post.PubDate = DateTimeOffset.Now;
                Post.LastModified = DateTimeOffset.Now;
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
                return Redirect("/Drafts");
            }

            return Page();
        }

        private void SavePost(Post post)
        {
            Post.Slug = _slugGenerator.CreateSlug(Post.Title);

            if (string.IsNullOrEmpty(Post.Excerpt))
            {
                Post.Excerpt = _excerptGenerator.CreateExcerpt(Post.Body, 140);
            }

            _dataStore.SavePost(Post);
        }
    }
}
