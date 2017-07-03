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
            //Post.Tags = Request.Form["Tags"][0].Replace(" ", "").Split(",").ToList();
            //Post.Slug = Post.Title.Replace(" ", "-");
            //string outputFilePath = $"{StorageFolder}\\{Post.Slug}";
            //int count = 0;
            //while (System.IO.File.Exists(outputFilePath))
            //{
            //    count++;
            //    outputFilePath = $"{StorageFolder}\\{Post.Slug}-{count}";

            //}
            //if (count != 0)
            //{
            //    Post.Slug = $"{Post.Slug}-{count}";
            //}
            //BlogDataStore dataStore = new BlogDataStore();
            //dataStore.SavePost(Post);
            //_blog.Posts.Add(Post);
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
            Post.Slug = Post.Title.Replace(" ", "-");
            string outputFilePath = $"{StorageFolder}\\{Post.Slug}";
            int count = 0;
            while (System.IO.File.Exists(outputFilePath))
            {
                count++;
                outputFilePath = $"{StorageFolder}\\{Post.Slug}-{count}";

            }
            if (count != 0)
            {
                Post.Slug = $"{Post.Slug}-{count}";
            }
            BlogDataStore dataStore = new BlogDataStore();
            dataStore.SavePost(Post);
            _blog.Posts.Add(Post);
        }
    }
}