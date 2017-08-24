using System;
using System.Collections.Generic;
using System.Text;
using BlogTemplate._1.Models;
using BlogTemplate._1.Pages;
using BlogTemplate._1.Services;
using BlogTemplate._1.Tests.Fakes;
using Xunit;
using static BlogTemplate._1.Pages.EditModel;

namespace BlogTemplate.Tests.Pages
{
    class EditTests
    {
        [Fact]
        public void UpdatePost_TitleIsUpdated_UpdateSlug()
        {
            IFileSystem testFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());
            SlugGenerator slugGenerator = new SlugGenerator(testDataStore);
            ExcerptGenerator excerptGenerator = new ExcerptGenerator();

            testDataStore.SavePost(new Post
            {
                Slug = "Title",
                IsPublic = true,
                PubDate = DateTimeOffset.Now,
            });

            EditedPostModel editedPost = new EditedPostModel()
            {
                Title = "Edited Title",
            };

            EditModel model = new EditModel(testDataStore, slugGenerator, excerptGenerator);

            model.OnPostPublish(id, true);

            testDataStore.UpdatePost(post, post.IsPublic);

            Assert.True(testFileSystem.FileExists($"BlogFiles\\Posts\\{newPost.PubDate.ToFileTime()}_{newPost.Id}.xml"));
            Post result = testDataStore.CollectPostInfo($"BlogFiles\\Posts\\{newPost.PubDate.ToFileTime()}_{newPost.Id}.xml");
            Assert.False(testFileSystem.FileExists($"BlogFiles\\Posts\\{oldPost.PubDate.ToFileTime()}_{oldPost.Id}.xml"));
        }
    }
}
