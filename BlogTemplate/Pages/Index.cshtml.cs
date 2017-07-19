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
        private BlogDataStore _dataStore;

        public IndexModel(Blog blog, BlogDataStore dataStore)
        {
            _blog = blog;
            _dataStore = dataStore;
        }

        public Blog Blog { get; set; }

        public void OnGet()
        {
            Blog = _blog;
            _blog.Posts = _dataStore.GetAllPosts();
        }
    }
}
