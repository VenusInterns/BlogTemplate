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
            oldPost = dataStore.GetPost(slug);

            if(oldPost == null)
            {
                RedirectToPage("/Index");
            }
        }

        public IActionResult OnPostPublish()
        {
            newPost.IsPublic = true;
            UpdatePost(newPost);
            return Redirect("/Index");
        }

        public IActionResult OnPostSaveDraft()
        {
            newPost.IsPublic = false;
            UpdatePost(newPost);
            return Redirect("/Index");
        }

        public void UpdatePost(Post newPost)
        {
            newPost.Tags = Request.Form["Tags"][0].Replace(" ", "").Split(",").ToList();

            BlogDataStore dataStore = new BlogDataStore();
            SlugGenerator slugGenerator = new SlugGenerator();
            newPost.Slug = slugGenerator.CreateSlug(newPost.Title);

            dataStore.UpdatePost(newPost, oldPost);

            //update post in blog's list of posts
            //might need to do in datastore instead
            int index = _blog.Posts.IndexOf(oldPost);
            _blog.Posts[index] = newPost;
        }
    }
}