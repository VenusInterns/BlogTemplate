using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using BlogTemplate._1.Models;
using BlogTemplate._1.Tests.Fakes;
using Microsoft.AspNetCore.Http;
using Xunit;
using static BlogTemplate._1.Pages.EditModel;

namespace BlogTemplate._1.Tests.Model
{
    public class BlogDataStoreTests
    {
        [Fact]
        public void SavePost_SetIdTwoPosts_UniqueIds()
        {
            IFileSystem testFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(testFileSystem);
            Post testPost1 = new Post
            {
                Slug = "Test-Post-Slug",
                Title = "Test Title",
                Body = "Test contents",
                IsPublic = true,
                PubDate = DateTime.UtcNow
            };
            Post testPost2 = new Post
            {
                Slug = "Test-Post-Slug",
                Title = "Test Title",
                Body = "Test contents",
                IsPublic = true,
                PubDate = DateTime.UtcNow
            };

            testDataStore.SavePost(testPost1);
            testDataStore.SavePost(testPost2);

            Assert.NotNull(testPost1.Id);
            Assert.NotNull(testPost2.Id);
            Assert.NotEqual(testPost1.Id, testPost2.Id);
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
            testPost.PubDate = DateTime.UtcNow;
            testDataStore.SavePost(testPost);

            Assert.True(testFileSystem.FileExists($"BlogFiles\\Posts\\{testPost.PubDate.UtcDateTime.ToString("s").Replace(":","-")}_{testPost.Id.ToString("N")}.xml"));
            Post result = testDataStore.GetPost(testPost.Id.ToString("N"));
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
                Body = "Test body",
                PubDate = DateTimeOffset.Now,
                IsPublic = true

            };
            testPost.Comments.Add(testComment);
            testDataStore.SavePost(testPost);

            string filePath = $"BlogFiles\\Posts\\{testPost.PubDate.UtcDateTime.ToString("s").Replace(":", "-")}_{testPost.Id.ToString("N")}.xml";
            Assert.True(testFileSystem.FileExists(filePath));
            StringReader xmlFileContents = new StringReader(testFileSystem.ReadFileText(filePath));
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
            Post result = testDataStore.GetPost(test.Id.ToString("N"));

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

            Assert.Null(testDataStore.GetPost("12345"));
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
                PubDate = DateTimeOffset.Now,
                LastModified = DateTimeOffset.Now,
                IsPublic = true,
                Excerpt = "Test excerpt"
            };
            var comment1 = new Comment
            {
                AuthorName = "Test name",
                Body = "test body",
                PubDate = DateTimeOffset.Now,
                IsPublic = true
            };
            var comment2 = new Comment
            {
                AuthorName = "Test name",
                Body = "test body",
                PubDate = DateTimeOffset.Now,
                IsPublic = true
            };
            testPost.Comments.Add(comment1);
            testPost.Comments.Add(comment2);
            testDataStore.SavePost(testPost);

            string text = testFileSystem.ReadFileText($"BlogFiles\\Posts\\{testPost.PubDate.UtcDateTime.ToString("s").Replace(":","-")}_{testPost.Id.ToString("N")}.xml");
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
                Body = "test body",
                PubDate = DateTimeOffset.Now,
                IsPublic = true
            };
            var comment2 = new Comment
            {
                AuthorName = "Test name",
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
        public void UpdatePost_TitleIsUpdated_UpdateSlug()
        {
            IFileSystem testFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(testFileSystem);

            Post oldPost = new Post
            {
                Slug = "Old-Title",
                IsPublic = true,
                PubDate = DateTimeOffset.Now
            };

            Post newPost = new Post
            {
                Slug = "New-Title",
                IsPublic = true,
                PubDate = oldPost.PubDate
            };

            testDataStore.SavePost(oldPost);
            newPost.Id = oldPost.Id;
            testDataStore.UpdatePost(newPost, true);

            Post result = testDataStore.CollectPostInfo($"BlogFiles\\Posts\\{newPost.PubDate.UtcDateTime.ToString("s").Replace(":","-")}_{newPost.Id.ToString("N")}.xml");
            Assert.Equal("New-Title", result.Slug);
        }

        [Fact]
        public void SaveFiles_CreatesFilesInUploadsFolder()
        {
            FakeFormFile file1 = new FakeFormFile();
            file1.FileName = "file1";
            file1.Length = 5;
            FakeFormFile file2 = new FakeFormFile();
            file2.FileName = "file2";
            file2.Length = 5;
            List<IFormFile> files = new List<IFormFile>();
            files.Add(file1);
            files.Add(file2);
            IFileSystem testFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(testFileSystem);
            testDataStore.SaveFiles(files);

            Assert.True(testFileSystem.FileExists("wwwroot\\Uploads\\file1"));
            Assert.True(testFileSystem.FileExists("wwwroot\\Uploads\\file2"));
        }

        [Fact]
        public void CollectPostInfo_EmptyFile_HasPostNode_SetDefaultValues()
        {
            IFileSystem testFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(testFileSystem);

            testFileSystem.WriteFileText($"BlogFiles\\Posts\\empty_file.xml", "<Post/>");
            Post testPost = testDataStore.CollectPostInfo($"BlogFiles\\Posts\\empty_file.xml");

            Assert.NotEqual(default(Guid), testPost.Id);
            Assert.Equal("", testPost.Slug);
            Assert.Equal("", testPost.Title);
            Assert.Equal("", testPost.Body);
            Assert.Equal(default(DateTimeOffset), testPost.PubDate);
            Assert.Equal(default(DateTimeOffset), testPost.LastModified);
            Assert.Equal(true, testPost.IsPublic);
            Assert.Equal(false, testPost.IsDeleted);
            Assert.Equal("", testPost.Excerpt);
        }

        [Fact]
        public void CollectPostInfo_EmptyFile_DoesNotHavePostNode_SetDefaultValues()
        {
            IFileSystem testFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(testFileSystem);

            testFileSystem.WriteFileText($"BlogFiles\\Posts\\empty_file.xml", "");

            Assert.Null(testDataStore.CollectPostInfo($"BlogFiles\\Posts\\empty_file.xml"));
        }
    }
}
