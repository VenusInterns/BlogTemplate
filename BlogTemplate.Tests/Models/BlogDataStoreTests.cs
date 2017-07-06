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

        [Fact]
        public void SaveComment_SaveSimpleComment()
        {
            BlogDataStore testDataStore = new BlogDataStore();
            Post testPost = new Post
            {
                Slug = "Test-slug",
                Title = "Test title",
                Body = "Test body",
                PubDate = DateTime.Now,
                LastModified = DateTime.Now,
                IsPublic = true,
                Excerpt = "Test excerpt"
            };

            Comment testComment = new Comment
            {
                AuthorName = "Test name",
                AuthorEmail = "Test email",
                Body = "Test body",
                PubDate = DateTime.Now,
                IsPublic = true

            };

            testDataStore.SavePost(testPost);
            testDataStore.SaveComment(testComment, testPost);

            Assert.NotEmpty(testPost.Comments);
            Assert.NotNull(testComment.PubDate);
            Assert.True(testComment.IsPublic);
            Assert.True(File.Exists("BlogFiles\\Test-slug.xml"));


        }

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
            //test.Comments.Add(comment);
            testDataStore.SavePost(test);
            //testDataStore.SaveComment(comment, test);
            Post result = testDataStore.GetPost("Test-Title");

            Assert.NotNull(result);
            Assert.Equal(result.Slug, "Test-Title");
            Assert.Equal(result.Body, "Test body");
            Assert.Equal(result.Title, "Test Title");
            Assert.NotNull(result.PubDate);
            Assert.NotNull(result.LastModified);
            Assert.True(result.IsPublic);
            Assert.Equal(result.Excerpt, "Test excerpt");
            //Assert.NotEmpty(result.Comments);
        }

        [Fact]
        public void GetPost_PostDNE_ReturnsNull()
        {
            BlogDataStore testDataStore = new BlogDataStore();

            Assert.Null(testDataStore.GetPost("does-not-exist"));
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
