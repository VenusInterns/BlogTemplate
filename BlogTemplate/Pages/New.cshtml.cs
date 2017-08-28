using System;
using System.ComponentModel.DataAnnotations;
using BlogTemplate._1.Models;
using BlogTemplate._1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

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
            int TotalPosts = _dataStore.GetAllPosts().Count + _dataStore.GetAllDrafts().Count;            
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
            Post post = new Post {
                Title = NewPost.Title,
                Body = NewPost.Body,
                Excerpt = NewPost.Excerpt,
                Slug = _slugGenerator.CreateSlug(NewPost.Title),
                LastModified = DateTimeOffset.Now,
            };

            if (string.IsNullOrEmpty(post.Excerpt))
            {
                post.Excerpt = _excerptGenerator.CreateExcerpt(post.Body, 140);
            }

            if (publishPost)
            {
                post.PubDate = DateTimeOffset.Now;
                post.IsPublic = true;
            }

            _dataStore.SaveFiles(Request.Form.Files.ToList());
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
