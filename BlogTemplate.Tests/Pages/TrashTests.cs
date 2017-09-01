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

namespace BlogTemplate.Tests.Pages
{
    public class TrashTests
    {
        [Fact]
        public void DeletePost_MoveToTrash()
        {
            IFileSystem fakeFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(fakeFileSystem);
            MarkdownRenderer markdownRenderer = new MarkdownRenderer();

            Post post = new Post
            {
                Title = "Title",
                Body = "This is the body of my post",
                IsDeleted = false,
            };
            testDataStore.SavePost(post);

            PostModel testPostModel = new PostModel(testDataStore, markdownRenderer);
            testPostModel.PageContext = new PageContext();
            testPostModel.OnPostDeletePost(post.Id);
            
            Assert.True(post.IsDeleted);
        }

        [Fact]
        public void UnDeletePost_MoveToIndex()
        {
            //Arrange
            IFileSystem fakeFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(fakeFileSystem);
            MarkdownRenderer markdownRenderer = new MarkdownRenderer();

            Post post = new Post
            {
                Title = "Title",
                Body = "This is the body of my post",
                IsDeleted = true,
            };

            testDataStore.SavePost(post);

            PostModel testPostModel = new PostModel(testDataStore, markdownRenderer);
            testPostModel.PageContext = new PageContext();

            //Act
            testPostModel.OnGet(post.Id);
            testPostModel.OnPostUnDeletePost(post.Id);


            IndexModel model = new IndexModel(testDataStore);
            model.OnGet();
            //Assert
            Assert.Equal(1, model.PostSummaries.Count());
        }
    }
}
