using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using BlogTemplate._1.Models;
using BlogTemplate._1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace BlogTemplate._1.Pages
{
    [Authorize]
    public class NewModel : PageModel
    {
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
                return Redirect("/Drafts");
            }

            return Page();
        }
        
        private void SavePost(NewPostViewModel newPost, bool publishPost)
        {
            if (string.IsNullOrEmpty(newPost.Excerpt))
            {
                newPost.Excerpt = _excerptGenerator.CreateExcerpt(newPost.Body);
            }

            Post post = new Post {
                Title = newPost.Title,
                Body = newPost.Body,
                Excerpt = newPost.Excerpt,
                Slug = _slugGenerator.CreateSlug(newPost.Title),
                LastModified = DateTimeOffset.Now,
                IsDeleted = false,
            };

            if (publishPost)
            {
                post.PubDate = DateTimeOffset.Now;
                post.IsPublic = true;
            }

            if (Request != null)
            {
                _dataStore.SaveFiles(Request.Form.Files.ToList());
            }

            _dataStore.SavePost(post);
        }

        public class NewPostViewModel
        {
            [Required]
            public string Title { get; set; }
            [Required]
            public string Body { get; set; }
            public string Excerpt { get; set; }
        }
    }
}
