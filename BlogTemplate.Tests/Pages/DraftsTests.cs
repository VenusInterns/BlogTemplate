using System;
using System.Collections.Generic;
using System.Linq;
using BlogTemplate._1.Models;
using BlogTemplate._1.Pages;
using BlogTemplate._1.Tests.Fakes;
using Xunit;

namespace BlogTemplate.Tests.Pages
{
    class DraftsTests
    {
        [Fact]
        public void GetDrafts_ShowDraftSummaries()
        {
            IFileSystem fakeFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(fakeFileSystem);

            Post draftPost = new Post
            {
                Slug = "Draft-Post",
                Title = "Draft Post",
                Excerpt = "Draft excerpt",
                IsPublic = false,
                PubDate = DateTime.UtcNow,
            };
            Post publishedPost = new Post
            {
                Slug = "Published-Post",
                Title = "Published Post",
                Excerpt = "Published excerpt",
                IsPublic = true,
                PubDate = DateTime.UtcNow,
            };

            testDataStore.SavePost(draftPost);
            testDataStore.SavePost(publishedPost);

            DraftsModel testDraftsModel = new DraftsModel(testDataStore);

            testDraftsModel.OnGet();

            Assert.Equal(1, testDraftsModel.DraftSummaries.Count());
            Assert.Equal(draftPost.Id, testDraftsModel.DraftSummaries.First().Id);
            Assert.Equal("Draft-Post", testDraftsModel.DraftSummaries.First().Slug);
            Assert.Equal("Draft Post", testDraftsModel.DraftSummaries.First().Title);
            Assert.Equal(draftPost.PubDate, testDraftsModel.DraftSummaries.First().PublishTime);
            Assert.Equal("Draft excerpt", testDraftsModel.DraftSummaries.First().Excerpt);
            Assert.Equal(0, testDraftsModel.DraftSummaries.First().CommentCount);
        }
    }
}
