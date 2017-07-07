using System;
using System.Collections.Generic;
using System.Text;
using BlogTemplate.Models;
using Xunit;
using System.IO;
using System.Linq;
using System.Xml.Linq;

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
            
            Assert.True(File.Exists("BlogFiles\\Test-slug.xml"));
            XDocument doc = XDocument.Load("BlogFiles\\Test-slug.xml");
            Assert.True(doc.Root.Elements("Comments").Any());
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
        public void CreateSlug_ReturnValidSlug()
        {
            BlogDataStore testDataStore = new BlogDataStore();
            SlugGenerator testSlug = new SlugGenerator();
            Post test = new Post
            {
                Title = "Test Title",
                Slug = testSlug.CreateSlug("Test Title"),
                Body = "Test body",
                PubDate = DateTime.Now,
                LastModified = DateTime.Now,
                IsPublic = true,
                Excerpt = "Test excerpt",
            };
            testDataStore.SavePost(test);


            Post test1 = new Post
            {
                Title = "Test Title",
                Slug = testSlug.CreateSlug("Test Title"),
                Body = "Test body",
                PubDate = DateTime.Now,
                LastModified = DateTime.Now,
                IsPublic = true,
                Excerpt = "Test excerpt",
            };
            testDataStore.SavePost(test1);

            Post test2 = new Post
            {
                Title = "Test Title",
                Slug = testSlug.CreateSlug("Test Title"),
                Body = "Test body",
                PubDate = DateTime.Now,
                LastModified = DateTime.Now,
                IsPublic = true,
                Excerpt = "Test excerpt",
            };
            testDataStore.SavePost(test2);

            Post result = testDataStore.GetPost("Test-Title");
            Assert.Equal("Test-Title", result.Slug);

            Post result1 = testDataStore.GetPost("Test-Title-1");
            Assert.Equal("Test-Title-1", result1.Slug);

            Post result2 = testDataStore.GetPost("Test-Title-1-2");
            Assert.Equal("Test-Title-1-2", result2.Slug);
        }


        [Fact]
        public void GetPost_PostDNE_ReturnsNull()
        {
            BlogDataStore testDataStore = new BlogDataStore();

            Assert.Null(testDataStore.GetPost("does-not-exist"));
        }

        [Fact]
        public void GetAllComments_ReturnsList()
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
            var comment1 = new Comment
            {
                AuthorName = "Test name",
                AuthorEmail = "Test email",
                Body = "test body",
                PubDate = DateTime.Now,
                IsPublic = true
            };
            var comment2 = new Comment
            {
                AuthorName = "Test name",
                AuthorEmail = "Test email",
                Body = "test body",
                PubDate = DateTime.Now,
                IsPublic = true
            };
            testDataStore.SavePost(testPost);
            testDataStore.SaveComment(comment1, testPost);
            testDataStore.SaveComment(comment2, testPost);

            List<Comment> comments = testDataStore.GetAllComments(testPost.Slug);
            Assert.NotEmpty(comments);
        }

        [Fact]
        public void GetAllPosts_ReturnsList()
        {
            BlogDataStore testDataStore = new BlogDataStore();
            Post post1 = new Post
            {
                Slug = "Test-slug",
                Title = "Test title",
                Body = "Test body",
                PubDate = DateTime.Now,
                LastModified = DateTime.Now,
                IsPublic = true,
                Excerpt = "Test excerpt"
            };
            Post post2 = new Post
            {
                Slug = "Test-slug",
                Title = "Test title",
                Body = "Test body",
                PubDate = DateTime.Now,
                LastModified = DateTime.Now,
                IsPublic = true,
                Excerpt = "Test excerpt"
            };
            testDataStore.SavePost(post1);
            testDataStore.SavePost(post2);

            List<Post> posts = testDataStore.GetAllPosts();
            Assert.NotEmpty(posts);
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
