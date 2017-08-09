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
        private Blog _blog;
        private BlogDataStore _dataStore;

        public NewModel(Blog blog, BlogDataStore dataStore)
        {
            _blog = blog;
            _dataStore = dataStore;
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
                Post.IsPublic = true;
                SavePost(Post);
                return Redirect("/Index");
            }

            return Page();
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostSaveDraft()
        {
            Post.IsPublic = false;
            SavePost(Post);
            return Redirect("/Index");
        }

        private void SavePost(Post post)
        {
            Post.Tags = Request.Form["Tags"][0].Replace(" ", "").Split(",").ToList();

            SlugGenerator slugGenerator = new SlugGenerator(_dataStore);
            Post.Slug = slugGenerator.CreateSlug(Post.Title);

            if (string.IsNullOrEmpty(Post.Excerpt))
            {
                ExcerptGenerator excerptGenerator = new ExcerptGenerator();
                Post.Excerpt = excerptGenerator.CreateExcerpt(Post.Body, 140);
            }

            _dataStore.SavePost(Post);
            _blog.Posts.Add(Post);
        }
    }
}

