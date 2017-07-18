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
        private BlogDataStore _dataStore;

        public NewModel(Blog blog, BlogDataStore dataStore)
        {
            _blog = blog;
            _dataStore = dataStore;
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
            if (Post.TagsString != null)
            {
                Post.Tags = Post.TagsString.Replace(" ", "").Split(",").ToList();
            }

            SlugGenerator slugGenerator = new SlugGenerator(_dataStore);
            Post.Slug = slugGenerator.CreateSlug(Post.Title);

            _dataStore.SavePost(Post);
            _blog.Posts.Add(Post);
        }
    }
}