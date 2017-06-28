using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BlogTemplate.Models;

namespace BlogTemplate.Pages
{
    public class NewModel : PageModel
    {
        private Blog _blog;
        public NewModel(Blog blog)
        {
            _blog = blog;
        }
        public void OnGet()
        {
        }

        [BindProperty]
        public Post Post { get; set; }

        public IActionResult OnPostPublish()
        {
            Post.Tags = Request.Form["Tags"][0].Replace(" ", "").Split(",").ToList();
            //Post.Slug = Post.Title.Replace(" ", "-");
            BlogDataStore dataStore = new BlogDataStore();
            dataStore.SavePost(Post);
            _blog.Posts.Add(Post);
            return Redirect("/Index");
        }
    }
}