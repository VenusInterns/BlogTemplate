using System;
using BlogTemplate._1.Models;
using BlogTemplate._1.Pages;
using BlogTemplate._1.Services;
using BlogTemplate._1.Tests.Fakes;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Xunit;

namespace BlogTemplate._1.Tests.Pages
{
    public class EditTests
    {
        [Fact]
        public void UpdatePost_PublishedToPublished_TitleIsUpdated_UpdateSlug()
        {
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());
            SlugGenerator slugGenerator = new SlugGenerator(testDataStore);
            ExcerptGenerator excerptGenerator = new ExcerptGenerator(140);

            Post post = new Post {
                Title = "Title",
                Slug = "Title",
                IsPublic = true,
                PubDate = DateTimeOffset.Now,
            };

            testDataStore.SavePost(post);

            EditModel testEditModel = new EditModel(testDataStore, slugGenerator, excerptGenerator);
            testEditModel.PageContext = new PageContext();
            testEditModel.EditedPost = new EditModel.EditedPostModel {
                Title = "Edited Title",
                Excerpt = "Excerpt",
            };

            testEditModel.OnPostPublish(post.Id.ToString("N"), true);

            post = testDataStore.GetPost(post.Id.ToString("N"));

            Assert.Equal("Edited-Title", post.Slug);
        }

        [Fact]
        public void UpdatePost_DraftToPublished_TitleIsUpdated_UpdateSlug()
        {
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());
            SlugGenerator slugGenerator = new SlugGenerator(testDataStore);
            ExcerptGenerator excerptGenerator = new ExcerptGenerator(140);

            Post post = new Post {
                Title = "Title",
                IsPublic = false,
            };

            testDataStore.SavePost(post);

            EditModel testEditModel = new EditModel(testDataStore, slugGenerator, excerptGenerator);
            testEditModel.PageContext = new PageContext();
            testEditModel.EditedPost = new EditModel.EditedPostModel {
                Title = "Edited Title",
                Excerpt = "Excerpt",
            };

            testEditModel.OnPostPublish(post.Id.ToString("N"), true);

            post = testDataStore.GetPost(post.Id.ToString("N"));

            Assert.Equal("Edited-Title", post.Slug);
        }

        [Fact]
        public void UpdatePost_PreviouslyPublishedDraftToPublished_TitleIsUpdated_UpdateSlug()
        {
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());
            SlugGenerator slugGenerator = new SlugGenerator(testDataStore);
            ExcerptGenerator excerptGenerator = new ExcerptGenerator(140);

            Post post = new Post {
                Title = "Title",
                Slug = "Title",
                IsPublic = false,
            };

            testDataStore.SavePost(post);

            EditModel testEditModel = new EditModel(testDataStore, slugGenerator, excerptGenerator);
            testEditModel.PageContext = new PageContext();
            testEditModel.EditedPost = new EditModel.EditedPostModel {
                Title = "Edited Title",
                Excerpt = "Excerpt",
            };

            testEditModel.OnPostPublish(post.Id.ToString("N"), true);

            post = testDataStore.GetPost(post.Id.ToString("N"));

            Assert.Equal("Edited-Title", post.Slug);
        }

        [Fact]
        public void UpdatePost_PreviouslyPublishedDraftToPublished_DoNotUpdatePubDate()
        {
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());
            SlugGenerator slugGenerator = new SlugGenerator(testDataStore);
            ExcerptGenerator excerptGenerator = new ExcerptGenerator(140);

            Post post = new Post {
                Title = "Title",
                Slug = "Title",
                IsPublic = false,
                PubDate = new DateTimeOffset(new DateTime(1997, 7, 3), TimeSpan.Zero),
            };

            testDataStore.SavePost(post);

            EditModel testEditModel = new EditModel(testDataStore, slugGenerator, excerptGenerator);
            testEditModel.PageContext = new PageContext();
            testEditModel.EditedPost = new EditModel.EditedPostModel {
                Title = "Edited Title",
                Excerpt = "Excerpt",
            };

            testEditModel.OnPostPublish(post.Id.ToString("N"), true);

            post = testDataStore.GetPost(post.Id.ToString("N"));

            Assert.Equal(new DateTimeOffset(new DateTime(1997, 7, 3), TimeSpan.Zero), post.PubDate);
        }

        [Fact]
        public void UpdatePost_PublishedToDraft_TitleIsUpdated_DoNotUpdateSlug()
        {
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());
            SlugGenerator slugGenerator = new SlugGenerator(testDataStore);
            ExcerptGenerator excerptGenerator = new ExcerptGenerator(140);

            Post post = new Post {
                Title = "Title",
                Slug = "Title",
                IsPublic = true,
            };

            testDataStore.SavePost(post);

            EditModel testEditModel = new EditModel(testDataStore, slugGenerator, excerptGenerator);
            testEditModel.PageContext = new PageContext();
            testEditModel.EditedPost = new EditModel.EditedPostModel {
                Title = "Edited Title",
                Excerpt = "Excerpt",
            };

            testEditModel.OnPostSaveDraft(post.Id.ToString("N"));

            post = testDataStore.GetPost(post.Id.ToString("N"));

            Assert.Equal("Title", post.Slug);
        }
    }
}
