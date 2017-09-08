using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlogTemplate._1.Models;
using BlogTemplate._1.Pages;
using BlogTemplate._1.Services;
using BlogTemplate._1.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Xunit;

namespace BlogTemplate._1.Tests.Pages
{
    public class TrashTests
    {
        [Fact]
        public void DeletePost_MoveToTrash()
        {
            IFileSystem fakeFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(fakeFileSystem);
            MarkdownRenderer markdownRenderer = new MarkdownRenderer();
            PostModel testPostModel = new PostModel(testDataStore, markdownRenderer);
            testPostModel.PageContext = new PageContext();

            Post post = new Post
            {
                Title = "Title",
                Body = "This is the body of my post",
                IsDeleted = false,
            };
            testDataStore.SavePost(post);

            testPostModel.OnPostDeletePost(post.Id.ToString("N"));
            Post result = testDataStore.GetPost(post.Id.ToString("N"));

            Assert.True(result.IsDeleted);
        }

        [Fact]
        public void UnDeletePost_MoveToIndex()
        {
            IFileSystem fakeFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(fakeFileSystem);
            MarkdownRenderer markdownRenderer = new MarkdownRenderer();
            PostModel testPostModel = new PostModel(testDataStore, markdownRenderer);
            testPostModel.PageContext = new PageContext();

            Post post = new Post
            {
                Title = "Title",
                Body = "This is the body of my post",
                IsDeleted = true,
            };
            testDataStore.SavePost(post);

            testPostModel.OnPostUnDeletePost(post.Id.ToString("N"));
            Post result = testDataStore.GetPost(post.Id.ToString("N"));

            Assert.False(result.IsDeleted);
        }
    }
}
