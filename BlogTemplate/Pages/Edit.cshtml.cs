using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BlogTemplate._1.Models;
using Microsoft.AspNetCore.Authorization;
using BlogTemplate.Services;


namespace BlogTemplate._1.Pages
{
    [Authorize]
    public class EditModel : PageModel
    {
        private BlogDataStore _dataStore;

        public EditModel(BlogDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        [BindProperty]
        public Post newPost { get; set; }

        public Post oldPost { get; set; }

        public void OnGet()
        {
            InitializePost();
        }

        private void InitializePost()
        {
            string slug = RouteData.Values["slug"].ToString();
            newPost = oldPost = _dataStore.GetPost(slug);

            if (oldPost == null)
            {
                RedirectToPage("/Index");
            }
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostPublish()
        {
            string slug = RouteData.Values["slug"].ToString();
            newPost.IsPublic = true;
            UpdatePost(newPost, slug);
            return Redirect($"/Post/{newPost.Slug}");
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostSaveDraft()
        {
            string slug = RouteData.Values["slug"].ToString();
            newPost.IsPublic = false;
            UpdatePost(newPost, slug);
            return Redirect("/Index");
        }

        private void UpdatePost(Post newPost, string slug)
        {
            oldPost = _dataStore.GetPost(slug);
            newPost.PubDate = oldPost.PubDate;
            if (newPost.Excerpt == null)
            {
                ExcerptGenerator excerptGenerator = new ExcerptGenerator();
                newPost.Excerpt = excerptGenerator.CreateExcerpt(newPost.Body, 140);
            }

            if (Request.Form["updateslug"] == "true")
            {
                SlugGenerator slugGenerator = new SlugGenerator(_dataStore);
                newPost.Slug = slugGenerator.CreateSlug(newPost.Title);
            }
            else
            {
                newPost.Slug = oldPost.Slug;
            }
            newPost.Comments = oldPost.Comments;

            _dataStore.UpdatePost(newPost, oldPost);
        }
    }
}
