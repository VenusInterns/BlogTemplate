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
            BlogDataStore dataStore = new BlogDataStore();
            oldPost = dataStore.GetPost(slug);
            newPost.Tags = Request.Form["Tags"][0].Replace(" ", "").Split(",").ToList();
            
            SlugGenerator slugGenerator = new SlugGenerator();
            newPost.Slug = slugGenerator.CreateSlug(newPost.Title);
            newPost.Comments = oldPost.Comments;

            dataStore.UpdatePost(newPost, oldPost);
        }
    }
}