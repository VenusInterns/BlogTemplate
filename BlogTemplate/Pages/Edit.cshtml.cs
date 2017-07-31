using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BlogTemplate.Models;

namespace BlogTemplate.Pages
{
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
            string slug = RouteData.Values["slug"].ToString();
            newPost = oldPost = _dataStore.GetPost(slug);

            if (oldPost == null)
            {
                RedirectToPage("/Index");
            }
        }

        public IActionResult OnPostPublish()
        {
            string slug = RouteData.Values["slug"].ToString();
            newPost.IsPublic = true;
            UpdatePost(newPost, slug);
            return Redirect($"/Post/{newPost.Slug}");
        }

        public IActionResult OnPostSaveDraft()
        {
            string slug = RouteData.Values["slug"].ToString();
            newPost.IsPublic = false;
            UpdatePost(newPost, slug);
            return Redirect("/Index");
        }

        public void UpdatePost(Post newPost, string slug)
        {
            oldPost = _dataStore.GetPost(slug);
            DateTime OrigPubDate = oldPost.PubDate;
            newPost.PubDate = OrigPubDate;
            newPost.Tags = Request.Form["Tags"][0].Replace(" ", "").Split(",").ToList();

            SlugGenerator slugGenerator = new SlugGenerator(_dataStore);
            newPost.Slug = slugGenerator.CreateSlug(newPost.Title);
            //if (newPost.Slug != oldPost.Slug)
            //{
            //    //SlugUpdateWarning();
            //    return confirm('Updated slug. Okay?');
            //}
            newPost.Comments = oldPost.Comments;

            _dataStore.UpdatePost(newPost, oldPost);
        }

        public IActionResult SlugUpdateWarning()
        {
            TempData["notice"] = "Updated slug";
            return Redirect("/Edit");
        }
    }
}