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
        public Blog Blog { get; set; }

        public EditModel(Blog blog)
        {
            _blog = blog;
        }

        [BindProperty]
        public Post newPost { get; set; }
        [BindProperty]
        public Post oldPost { get; set; }
        public void OnGet()
        {
            InitializePost();
        }

        private void InitializePost()
        {
            string slug = RouteData.Values["slug"].ToString();
            BlogDataStore dataStore = new BlogDataStore();
            newPost = oldPost = dataStore.GetPost(slug);

            if(oldPost == null)
            {
                RedirectToPage("/Index");
            }
        }

        public IActionResult OnPostPublish()
        {
            newPost.IsPublic = true;
            UpdatePost(newPost, oldPost);
            return Redirect($"/Post/{newPost.Slug}");
        }

        public IActionResult OnPostSaveDraft()
        {
            newPost.IsPublic = false;
            UpdatePost(newPost, oldPost);
            return Redirect("/Index");
        }

        public void UpdatePost(Post newPost, Post oldPost)
        {
            newPost.Tags = Request.Form["Tags"][0].Replace(" ", "").Split(",").ToList();

            BlogDataStore dataStore = new BlogDataStore();
            SlugGenerator slugGenerator = new SlugGenerator();
            newPost.Slug = slugGenerator.CreateSlug(newPost.Title);

            dataStore.UpdatePost(newPost, oldPost);
        }
    }
}