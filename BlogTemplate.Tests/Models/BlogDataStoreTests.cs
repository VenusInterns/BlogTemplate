using System;
using System.Collections.Generic;
using System.Text;
using BlogTemplate.Models;
using Xunit;
using System.IO;

namespace BlogTemplate.Tests.Model
{
    public class BlogDataStoreTests : IDisposable
    {
        [Fact]
        public void SavePost_SaveSimplePost()
        {
            // Arrange
            BlogDataStore testDataStore = new BlogDataStore();
            Post testPost = new Post {
                Slug = "Test-Post-Slug",
                Title = "Test Title",
                Body = "Test contents",
            };

            // Act
            testDataStore.SavePost(testPost);

            // Assert
            Assert.True(File.Exists("BlogFiles\\Test-Post-Slug.xml"));
            Post result = testDataStore.GetPost("Test-Post-Slug");
            Assert.Equal("Test-Post-Slug", result.Slug);
            Assert.Equal("Test Title", result.Title);
            Assert.Equal("Test contents", result.Body);
        }

        //[Fact]
        //public void SavePost_DuplicatePostSlug_ShouldAppendNumber()
        //{
        //    // Arrange
        //    BlogDataStore testDataStore = new BlogDataStore();
        //    Post testPost1 = new Post
        //    {
        //        Slug = "Test-Post-Slug",
        //        Title = "Test Title",
        //        Body = "Test contents",
        //    };
        //    Post testPost2 = new Post
        //    {
        //        Slug = "Test-Post-Slug",
        //        Title = "Test Title 2",
        //        Body = "Test contents 2",
        //    };
        //    testDataStore.SavePost(testPost1);
        //    testDataStore.SavePost(testPost2);

        //    // Act and Assert
        //    Assert.True(File.Exists("BlogFiles\\Test-Post-Slug.xml"));
        //    Assert.True(File.Exists("BlogFiles\\Test-Post-Slug-1.xml"));
        //    Assert.NotEqual(testPost1.Slug, testPost2.Slug);
        //}

        [Fact]
        public void GetPost_FindPostBySlug_ReturnsPost()
        {
            BlogDataStore testDataStore = new BlogDataStore();
            var comment = new Comment
            {
                AuthorName = "Test name",
                AuthorEmail = "Test email",
                Body = "test body",
                PubDate = DateTime.Now,
                IsPublic = true
            };
            Post test = new Post
            {
                Slug = "Test-Title",
                Title = "Test Title",
                Body = "Test body",
                PubDate = DateTime.Now,
                LastModified = DateTime.Now,
                IsPublic = true,
                Excerpt = "Test excerpt",
            };
            test.Comments.Add(comment);
            testDataStore.SavePost(test);
            Post result = testDataStore.GetPost("Test-Title");

            Assert.NotNull(result);
            Assert.Equal(result.Slug, "Test-Title");
            Assert.Equal(result.Body, "Test body");
            Assert.Equal(result.Title, "Test Title");
            Assert.NotNull(result.PubDate);
            Assert.NotNull(result.LastModified);
            Assert.True(result.IsPublic);
            Assert.Equal(result.Excerpt, "Test excerpt");
            Assert.NotEmpty(result.Comments);
        }

        public void Dispose()
        {
            // Delete all the files we created along the way
            foreach(string file in Directory.EnumerateFiles("BlogFiles"))
            {
                File.Delete(file);
            }
        }
    }
}
