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
        public void SavePost_DuplicatePostSlug_ShouldThrow()
        {
            // Arrange
            BlogDataStore testDataStore = new BlogDataStore();
            Post testPost1 = new Post
            {
                Slug = "Test-Post-Slug",
                Title = "Test Title",
                Body = "Test contents",
            };
            Post testPost2 = new Post
            {
                Slug = "Test-Post-Slug",
                Title = "Test Title 2",
                Body = "Test contents 2",
            };
            testDataStore.SavePost(testPost1);

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => testDataStore.SavePost(testPost2));
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
