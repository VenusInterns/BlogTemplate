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
        private Blog _blog;
        private BlogDataStore _dataStore;

        public EditModel(Blog blog, BlogDataStore dataStore)
        {
            _blog = blog;
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
            int id = Convert.ToInt32(RouteData.Values["id"]);
            newPost = oldPost = _dataStore.GetPost(id);

            if (oldPost == null)
            {
                RedirectToPage("/Index");
            }
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostPublish()
        {
            int id = Convert.ToInt32(RouteData.Values["id"]);
            oldPost = _dataStore.GetPost(id);
            newPost.IsPublic = true;
            UpdatePost(id);
            return Redirect($"/Post/{id}/{newPost.Slug}");
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostSaveDraft()
        {
            int id = Convert.ToInt32(RouteData.Values["id"]);
            oldPost = _dataStore.GetPost(id);
            newPost.IsPublic = false;
            UpdatePost(id);
            return Redirect("/Index");
        }

        private void UpdatePost(int id)
        {
            newPost.Id = id;
            oldPost = _dataStore.GetPost(id);

            if(oldPost.PubDate.Equals(default(DateTime)))
            {
                if(newPost.IsPublic == true)
                {
                    newPost.PubDate = DateTime.UtcNow;
                }
            }
            else
            {
                newPost.PubDate = oldPost.PubDate;
            }

            newPost.Tags = Request.Form["Tags"][0].Replace(" ", "").Split(",").ToList();
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
