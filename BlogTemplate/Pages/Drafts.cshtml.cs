using System.Collections.Generic;
using System.Linq;
using BlogTemplate._1.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogTemplate._1.Pages
{
    public class DraftsModel : PageModel
    {
        const string StorageFolder = "BlogFiles";

        private BlogDataStore _dataStore;

        public DraftsModel(BlogDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public IEnumerable<Post> Posts { get; set; }

        public void OnGet()
        {
            Posts = _dataStore.GetAllPosts().Where(post => !post.IsPublic);
        }
    }
}
