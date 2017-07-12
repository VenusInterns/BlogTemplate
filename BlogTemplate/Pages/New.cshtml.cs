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
        const string StorageFolder = "BlogFiles";
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
            Post.IsPublic = true;
            SavePost(Post);
            return Redirect("/Index");
        }

        public IActionResult OnPostSaveDraft()
        {
            Post.IsPublic = false;
            SavePost(Post);
            return Redirect("/Index");
        }

        public void SavePost(Post post)
        {
            Post.Tags = Request.Form["Tags"][0].Replace(" ", "").Split(",").ToList();

            BlogDataStore dataStore = new BlogDataStore();
            SlugGenerator slugGenerator = new SlugGenerator();
            Post.Slug = slugGenerator.CreateSlug(Post);

            dataStore.SavePost(Post);
            _blog.Posts.Add(Post);
        }
    }
}