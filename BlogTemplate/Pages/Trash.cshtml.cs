using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogTemplate._1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogTemplate._1.Pages
{
    public class TrashModel : PageModel
    {
        const string StorageFolder = "BlogFiles";

        private readonly BlogDataStore _dataStore;

        public IEnumerable<PostSummaryModel> PostSummaries { get; private set; }

        public TrashModel(BlogDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public void OnGet()
        {
            Func<Post, bool> deletedPostFilter = p => p.IsDeleted;
            IEnumerable<Post> postModels = _dataStore.GetAllPosts().Where(deletedPostFilter);

            PostSummaries = postModels.Select(p => new PostSummaryModel
            {
                Id = p.Id,
                Slug = p.Slug,
                Title = p.Title,
                Excerpt = p.Excerpt,
                PublishTime = p.PubDate,
                CommentCount = p.Comments.Where(c => c.IsPublic).Count(),
            });
        }

        public class PostSummaryModel
        {
            public Guid Id { get; set; }
            public string Slug { get; set; }
            public string Title { get; set; }
            public DateTimeOffset PublishTime { get; set; }
            public string Excerpt { get; set; }
            public int CommentCount { get; set; }
        }
    }
}
