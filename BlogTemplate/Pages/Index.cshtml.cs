using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BlogTemplate.Models;
using System.IO;

namespace BlogTemplate.Pages
{
    public class IndexModel : PageModel
    {
        const string StorageFolder = "BlogFiles";

        private Blog _blog;

        public IndexModel(Blog blog)
        {
            _blog = blog;
        }

        public Blog Blog { get; set; }

        public void OnGet()
        {
            Blog = _blog;
            BlogDataStore dataStore = new BlogDataStore();
            _blog.Posts = dataStore.GetAllPosts();
        }
    }
}
