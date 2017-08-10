using System;
using System.Collections.Generic;
using System.Text;
using BlogTemplate.Models;
using Xunit;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using BlogTemplate.Tests.Fakes;

namespace BlogTemplate.Tests.Model
{
    public class BlogDataStoreTests
    {
        [Fact]
        public void SetId_SetIdTwoPosts_IncrementsId()
        {
            IFileSystem testFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(testFileSystem);
            Post testPost1 = new Post
            {
                Slug = "Test-Post-Slug",
                Title = "Test Title",
                Body = "Test contents",
            };
            Post testPost2 = new Post
            {
                Slug = "Test-Post-Slug",
                Title = "Test Title",
                Body = "Test contents",
            };

            testDataStore.SetId(testPost1);
            testDataStore.SetId(testPost2);

            Assert.Equal(testPost1.Id, 1);
            Assert.Equal(testPost2.Id, 2);
        }

        [Fact]
        public void SavePost_SaveSimplePost()
        {
            IFileSystem testFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(testFileSystem);
            Post testPost = new Post
            {
                Slug = "Test-Post-Slug",
                Title = "Test Title",
                Body = "Test contents",
                IsPublic = true
            };
            testDataStore.SetId(testPost);
            testPost.PubDate = DateTime.UtcNow;
            testDataStore.SavePost(testPost);

            Assert.True(testFileSystem.FileExists($"BlogFiles\\{testPost.PubDate.ToFileTimeUtc()}_{testPost.Id}.xml"));
            Post result = testDataStore.GetPost(testPost.Id);
            Assert.Equal("Test-Post-Slug", result.Slug);
            Assert.Equal("Test Title", result.Title);
            Assert.Equal("Test contents", result.Body);
        }

        [Fact]
        public void SaveComment_SaveSimpleComment()
        {
            IFileSystem testFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(testFileSystem);
            Post testPost = new Post
            {
                Slug = "Test-slug",
                Title = "Test title",
                Body = "Test body",
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
            testDataStore.SetId(testPost);
            testPost.PubDate = DateTime.UtcNow;
            testDataStore.SavePost(testPost);

            Assert.True(testFileSystem.FileExists($"BlogFiles\\{testPost.PubDate.ToFileTimeUtc()}_{testPost.Id}.xml"));
            StringReader xmlFileContents = new StringReader(testFileSystem.ReadFileText($"BlogFiles\\{testPost.PubDate.ToFileTimeUtc()}_{testPost.Id}.xml"));
            XDocument doc = XDocument.Load(xmlFileContents);
            Assert.True(doc.Root.Elements("Comments").Any());
        }

        [Fact]
        public void GetPost_FindPostBySlug_ReturnsPost()
        {
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());
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
            testDataStore.SetId(test);
            test.Comments.Add(comment);
            testDataStore.SavePost(test);
            Post result = testDataStore.GetPost(test.Id);

            Assert.NotNull(result);
            Assert.Equal(result.Slug, "Test-Title");
            Assert.Equal(result.Body, "Test body");
            Assert.Equal(result.Title, "Test Title");
            Assert.NotNull(result.PubDate);
            Assert.NotNull(result.LastModified);
            Assert.True(result.IsPublic);
            Assert.Equal(result.Excerpt, "Test excerpt");
        }

        [Fact]
        public void GetPost_PostDNE_ReturnsNull()
        {
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());

            Assert.Null(testDataStore.GetPost(12345));
        }

        [Fact]
        public void GetAllComments_ReturnsList()
        {
            IFileSystem testFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(testFileSystem);
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
            testPost.Comments.Add(comment1);
            testPost.Comments.Add(comment2);
            testDataStore.SavePost(testPost);

            string text = testFileSystem.ReadFileText($"BlogFiles\\{testPost.PubDate.ToFileTimeUtc()}_{testPost.Id}.xml");
            StringReader reader = new StringReader(text);

            XDocument doc = XDocument.Load(reader);
            List<Comment> comments = testDataStore.GetAllComments(doc);
            Assert.NotEmpty(comments);
        }

        [Fact]
        public void GetAllPosts_ReturnsList()
        {
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());
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

        [Fact]
        public void FindComment_SwitchIsPublicValue()
        {

            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());
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
            testPost.Comments.Add(comment1);
            testPost.Comments.Add(comment2);
            testDataStore.SavePost(testPost);

            Comment newcom = testDataStore.FindComment(comment1.UniqueId, testPost);

            Assert.Equal(testPost.Comments.Count, 2);
            Assert.Equal(newcom.UniqueId, comment1.UniqueId);
        }
    }
}
