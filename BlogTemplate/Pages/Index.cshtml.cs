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

        private BlogDataStore _dataStore;
        public IEnumerable<PostSummaryModel> PostSummaries { get; private set; }

        public IndexModel(BlogDataStore dataStore)
        {
            _dataStore = dataStore;
        }


        public void OnGet()
        {
            Func<Post, bool> postFilter = p => p.IsPublic;
            if(User.Identity.IsAuthenticated)
            {
                postFilter = p => true;
            }

            IEnumerable<Post> postModels = _dataStore.GetAllPosts().Where(postFilter);

            PostSummaries = postModels.Select(p => new PostSummaryModel {
                Slug = p.Slug,
                Title = p.Title,
                Excerpt = p.Excerpt,
                PublishTime = p.PubDate,
                CommentCount = p.Comments.Count,
            });
        }

        public class PostSummaryModel
        {
            public string Slug { get; set; }
            public string Title { get; set; }
            public DateTime PublishTime { get; set; }
            public string Excerpt { get; set; }
            public int CommentCount { get; set; }

       }
    }
}
