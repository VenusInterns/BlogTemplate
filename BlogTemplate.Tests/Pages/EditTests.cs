using System;
using System.Collections.Generic;
using System.Text;
using BlogTemplate._1.Models;
using BlogTemplate._1.Pages;
using BlogTemplate._1.Services;
using BlogTemplate._1.Tests.Fakes;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Xunit;
using static BlogTemplate._1.Pages.EditModel;

namespace BlogTemplate._1.Tests.Pages
{
    public class EditTests
    {
        [Fact]
        public void UpdatePost_PostToPostTitleIsUpdated_UpdateSlug()
        {
            IFileSystem testFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());
            SlugGenerator slugGenerator = new SlugGenerator(testDataStore);
            ExcerptGenerator excerptGenerator = new ExcerptGenerator();

            Post post = new Post()
            {
                Title = "Title",
                Slug = "Title",
                IsPublic = true,
                PubDate = DateTimeOffset.Now,
            };

            testDataStore.SavePost(post);

            EditModel testEditModel = new EditModel(testDataStore, slugGenerator, excerptGenerator);
            testEditModel.PageContext = new PageContext();
            testEditModel.OnGet(post.Id);
            testEditModel.EditedPost.Title = "Edited Title";
            testEditModel.OnPostPublish(post.Id, true);

            post = testDataStore.GetPost(post.Id);

            Assert.Equal("Edited-Title", post.Slug);
        }


        [Fact]
        public void UpdatePost_PostToPostExcerptIsDeleted_GenerateNewExcerpt()
        {
            IFileSystem testFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());
            SlugGenerator slugGenerator = new SlugGenerator(testDataStore);
            ExcerptGenerator excerptGenerator = new ExcerptGenerator();

            Post post = new Post()
            {
                Title = "Title",
                Body = "Original body",
                Excerpt = "Original body",
                IsPublic = true,
                PubDate = DateTimeOffset.Now,
            };

            testDataStore.SavePost(post);

            EditModel testEditModel = new EditModel(testDataStore, slugGenerator, excerptGenerator);
            testEditModel.PageContext = new PageContext();
            testEditModel.OnGet(post.Id);
            testEditModel.EditedPost.Body = "Edited body";
            testEditModel.EditedPost.Excerpt = "";
            testEditModel.OnPostPublish(post.Id, true);

            post = testDataStore.GetPost(post.Id);

            Assert.Equal("Edited body", post.Excerpt);
        }
    }
}
