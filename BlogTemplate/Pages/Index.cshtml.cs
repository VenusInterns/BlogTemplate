using System;
using System.Collections.Generic;
using System.Linq;
using BlogTemplate._1.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogTemplate._1.Pages
{
    public class IndexModel : PageModel
    {
        const string StorageFolder = "BlogFiles";

        private readonly BlogDataStore _dataStore;

        public IEnumerable<PostSummaryModel> PostSummaries { get; private set; }

        public IndexModel(BlogDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public void OnGet()
        {
            Func<Post, bool> postFilter = p => p.IsPublic;
            IEnumerable<Post> postModels = _dataStore.GetAllPosts().Where(postFilter);

            PostSummaries = postModels.Select(p => new PostSummaryModel {
                Id = p.Id,
                Slug = p.Slug,
                Title = p.Title,
                Excerpt = p.Excerpt,
                PublishTime = p.PubDate,
                CommentCount = p.Comments.Count,
            });
        }

        public class PostSummaryModel
        {
            public int Id { get; set; }
            public string Slug { get; set; }
            public string Title { get; set; }
            public DateTimeOffset PublishTime { get; set; }
            public string Excerpt { get; set; }
            public int CommentCount { get; set; }

       }
    }
}
