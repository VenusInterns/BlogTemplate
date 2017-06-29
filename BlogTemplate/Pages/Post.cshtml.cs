using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BlogTemplate.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogTemplate.Pages
{
    public class PostModel : PageModel
    {
        private Blog _blog;

        public PostModel(Blog blog)
        {
            _blog = blog;
        }

        [BindProperty]
        public Comment Comment { get; set; }

        public Post Post { get; set; }

        public void OnGet()
        {
            InitializePost();
        }

        private void InitializePost()
        {
            string slug = RouteData.Values["slug"].ToString();
            //Post = _blog.Posts.FirstOrDefault(p => p.Slug == slug);

            BlogDataStore dataStore = new BlogDataStore();
            Post = dataStore.GetPost(slug);

            if(Post == null)
            {
                 RedirectToPage("/Index");
            }
        }

        public IActionResult OnPostPublish()
        {
            InitializePost();
            if (ModelState.IsValid)
            {
                Post.Comments.Add(Comment);
            }
            return Page();
        }
    }
}
