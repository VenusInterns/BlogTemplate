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

        [BindProperty]
        public NewPostViewModel NewPost { get; set; }

        public void OnGet()
        {
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostPublish()
        {
            if (ModelState.IsValid)
            {
                SavePost(NewPost, true);
                return Redirect("/Index");
            }

            return Page();
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostSaveDraft()
        {
            if(ModelState.IsValid)
            {
                SavePost(NewPost, false);
                return Redirect("/Index");
            }

            return Page();
        }

        private void SavePost(NewPostViewModel newPost, bool publishPost)
        {
            Post post = new Post {
                Title = NewPost.Title,
                Body = NewPost.Body,
                Excerpt = NewPost.Excerpt,
                Slug = _slugGenerator.CreateSlug(NewPost.Title),
                LastModified = DateTime.UtcNow,
            };

            if (string.IsNullOrEmpty(post.Excerpt))
            {
                post.Excerpt = _excerptGenerator.CreateExcerpt(post.Body, 140);
            }

            if (publishPost)
            {
                post.PubDate = DateTime.UtcNow;
                post.IsPublic = true;
            }

            _dataStore.SavePost(post);
        }

        public class NewPostViewModel
        {
            public string Title { get; set; }
            public string Body { get; set; }
            public string Excerpt { get; set; }
        }
    }
}
