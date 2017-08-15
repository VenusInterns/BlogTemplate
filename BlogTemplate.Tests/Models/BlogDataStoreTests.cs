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
        public void SavePost_SaveSimplePost()
        {
            IFileSystem testFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(testFileSystem);
            Post testPost = new Post {
                Slug = "Test-Post-Slug",
                Title = "Test Title",
                Body = "Test contents",
            };

            testDataStore.SavePost(testPost);

            Assert.True(testFileSystem.FileExists("BlogFiles\\Test-Post-Slug.xml"));
            Post result = testDataStore.GetPost("Test-Post-Slug");
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
                PubDate = DateTimeOffset.Now,
                LastModified = DateTimeOffset.Now,
                IsPublic = true,
                Excerpt = "Test excerpt"
            };

            Comment testComment = new Comment
            {
                AuthorName = "Test name",
                AuthorEmail = "Test email",
                Body = "Test body",
                PubDate = DateTimeOffset.Now,
                IsPublic = true

            };

            testDataStore.SavePost(testPost);

            Assert.True(testFileSystem.FileExists("BlogFiles\\Test-slug.xml"));
            StringReader xmlFileContents = new StringReader(testFileSystem.ReadFileText("BlogFiles\\Test-slug.xml"));
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
                PubDate = DateTimeOffset.Now,
                IsPublic = true
            };
            Post test = new Post
            {
                Slug = "Test-Title",
                Title = "Test Title",
                Body = "Test body",
                PubDate = DateTimeOffset.Now,
                LastModified = DateTimeOffset.Now,
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
        }

        [Fact]
        public void GetPost_PostDNE_ReturnsNull()
        {
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());

            Assert.Null(testDataStore.GetPost("does-not-exist"));
        }

        [Fact]
        public void GetAllComments_ReturnsList()
        {
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());
            Post testPost = new Post
            {
                Slug = "Test-slug",
                Title = "Test title",
                Body = "Test body",
                PubDate = DateTimeOffset.Now,
                LastModified = DateTimeOffset.Now,
                IsPublic = true,
                Excerpt = "Test excerpt"
            };
            var comment1 = new Comment
            {
                AuthorName = "Test name",
                AuthorEmail = "Test email",
                Body = "test body",
                PubDate = DateTimeOffset.Now,
                IsPublic = true
            };
            var comment2 = new Comment
            {
                AuthorName = "Test name",
                AuthorEmail = "Test email",
                Body = "test body",
                PubDate = DateTimeOffset.Now,
                IsPublic = true
            };
            testPost.Comments.Add(comment1);
            testPost.Comments.Add(comment2);
            testDataStore.SavePost(testPost);

            List<Comment> comments = testDataStore.GetAllComments(testPost.Slug);
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
                PubDate = DateTimeOffset.Now,
                LastModified = DateTimeOffset.Now,
                IsPublic = true,
                Excerpt = "Test excerpt"
            };
            Post post2 = new Post
            {
                Slug = "Test-slug",
                Title = "Test title",
                Body = "Test body",
                PubDate = DateTimeOffset.Now,
                LastModified = DateTimeOffset.Now,
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
                PubDate = DateTimeOffset.Now,
                LastModified = DateTimeOffset.Now,
                IsPublic = true,
                Excerpt = "Test excerpt"
            };
            var comment1 = new Comment
            {
                AuthorName = "Test name",
                AuthorEmail = "Test email",
                Body = "test body",
                PubDate = DateTimeOffset.Now,
                IsPublic = true
            };
            var comment2 = new Comment
            {
                AuthorName = "Test name",
                AuthorEmail = "Test email",
                Body = "test body",
                PubDate = DateTimeOffset.Now,
                IsPublic = true
            };
            testPost.Comments.Add(comment1);
            testPost.Comments.Add(comment2);
            testDataStore.SavePost(testPost);

            Comment newcom = testDataStore.FindComment(comment1.UniqueId, testPost);

            Assert.Equal(testPost.Comments.Count, 2);
            Assert.Equal(newcom.UniqueId, comment1.UniqueId);
        }

        [Fact]
        public void UpdatePost_ChangePost_UpdatesXMLFile()
        {
            IFileSystem fakeFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(fakeFileSystem);

            Post oldPost = new Post
            {
                Slug = "Old-Title",
                Title = "Old Title",
                Body = "Old body",
                IsPublic = true,
                Excerpt = "Old excerpt"
            };

            Post newPost = new Post
            {
                Slug = "New-Title",
                Title = "New Title",
                Body = "New body",
                IsPublic = true,
                Excerpt = "New excerpt"
            };

            testDataStore.SavePost(oldPost);
            testDataStore.UpdatePost(newPost, oldPost);

            Assert.True(fakeFileSystem.FileExists($"BlogFiles\\New-Title.xml"));
            Post result = testDataStore.CollectPostInfo($"BlogFiles\\New-Title.xml");
            Assert.Equal(result.Slug, "New-Title");
            Assert.Equal(result.Title, "New Title");
            Assert.Equal(result.Body, "New body");
            Assert.True(result.IsPublic);
            Assert.Equal(result.Excerpt, "New excerpt");
        }

        [Fact]
        public void UpdatePost_ChangePost_DoesNotRemoveComments()
        {
            IFileSystem testFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(testFileSystem);

            Post oldPost = new Post
            {
                Slug = "Old-Title",
                Title = "Old Title",
                Body = "Old body",
                IsPublic = true,
                Excerpt = "Old excerpt"
            };
            Comment comment = new Comment
            {
                AuthorName = "Test name",
                AuthorEmail = "Test email",
                Body = "test body",
                PubDate = DateTimeOffset.Now,
                IsPublic = true
            };
            Post newPost = new Post
            {
                Slug = "New-Title",
                Title = "New Title",
                Body = "New body",
                IsPublic = true,
                Excerpt = "New excerpt"
            };

            oldPost.Comments.Add(comment);
            testDataStore.SavePost(oldPost);
            newPost.Comments = oldPost.Comments;
            testDataStore.UpdatePost(newPost, oldPost);
            Post result = testDataStore.GetPost(newPost.Slug);
            List<Comment> comments = testDataStore.GetAllComments(newPost.Slug);

            Assert.True(testFileSystem.FileExists(@"BlogFiles\New-Title.xml"));
            Assert.False(testFileSystem.FileExists(@"BlogFiles\Old-Title.xml"));
            Assert.NotEmpty(comments);
        }

        [Fact]
        public void UpdatePost_TitleIsUpdated_UpdateSlug()
        {
            IFileSystem testFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(testFileSystem);

            Post oldPost = new Post
            {
                Slug = "Old-Title",
            };

            Post newPost = new Post
            {
                Slug = "New-Title",
            };

            testDataStore.SavePost(oldPost);
            testDataStore.UpdatePost(newPost, oldPost);

            Assert.True(testFileSystem.FileExists($"BlogFiles\\New-Title.xml"));
            Post result = testDataStore.CollectPostInfo($"BlogFiles\\New-Title.xml");
            Assert.False(testFileSystem.FileExists($"BlogFiles\\Old-Title.xml"));
        }

        [Fact]
        public void UpdatePost_TitleIsUpdated_KeepSlug()
        {
            IFileSystem testFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(testFileSystem);

            Post oldPost = new Post
            {
                Slug = "Old-Title",
                Title = "Old Title",
            };

            Post newPost = new Post
            {
                Slug = "Old-Title",
                Title = "New Title",
            };

            testDataStore.SavePost(oldPost);
            testDataStore.UpdatePost(newPost, oldPost);

            Assert.True(testFileSystem.FileExists($"BlogFiles\\Old-Title.xml"));
            Post result = testDataStore.CollectPostInfo($"BlogFiles\\Old-Title.xml");
            Assert.Equal(result.Title, "New Title");
        }
    }
}
