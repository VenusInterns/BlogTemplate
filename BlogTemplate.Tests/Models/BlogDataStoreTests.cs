using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using BlogTemplate._1.Models;
using BlogTemplate._1.Tests.Fakes;
using Xunit;
using static BlogTemplate._1.Pages.EditModel;

namespace BlogTemplate._1.Tests.Model
{
    public class BlogDataStoreTests
    {
        [Fact]
        public void SavePost_SetIdTwoPosts_IncrementsId()
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

            Assert.Equal(testPost2.Id, testPost1.Id + 1);
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

            Assert.True(testFileSystem.FileExists($"BlogFiles\\Posts\\{testPost.PubDate.UtcDateTime.ToString("s").Replace(":","-")}_{testPost.Id}.xml"));
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

            string filePath = $"BlogFiles\\Posts\\{testPost.PubDate.UtcDateTime.ToString("s").Replace(":", "-")}_{testPost.Id}.xml";
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

            string text = testFileSystem.ReadFileText($"BlogFiles\\Posts\\{testPost.PubDate.UtcDateTime.ToString("s").Replace(":","-")}_{testPost.Id}.xml");
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


        class TestDataStore : BlogDataStore
        {
            public TestDataStore(IFileSystem fileSystem) : base(fileSystem)
            {
            }

            public static void ResetCurrentId()
            {
                CurrentId = 0;
            }
        }

        [Fact]
        public void Constructor_PostExists_IncrementsNextId()
        {
            FakeFileSystem fileSystem = new FakeFileSystem();
            fileSystem.AddFile("BlogFiles\\Posts\\date_1.xml");
            TestDataStore.ResetCurrentId();
            TestDataStore dataStore = new TestDataStore(fileSystem);
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
            dataStore.SavePost(testPost);

            Assert.Equal(2, testPost.Id);
        }

        [Fact]
        public void Constructor_DraftExists_IncrementsNextId()
        {
            FakeFileSystem fileSystem = new FakeFileSystem();
            fileSystem.AddFile("BlogFiles\\Drafts\\1.xml");
            TestDataStore.ResetCurrentId();
            TestDataStore dataStore = new TestDataStore(fileSystem);
            Post testPost = new Post
            {
                Title = "Test",
                Slug = "Test",
                Body = "test body",
                PubDate = DateTimeOffset.Now,
                IsPublic = false
            };
            dataStore.SavePost(testPost);

            Assert.Equal(2, testPost.Id);
        }

        [Fact]
        public void Constructor_PostAndDraftExist_IncrementsNextId()
        {
            FakeFileSystem fileSystem = new FakeFileSystem();
            fileSystem.AddFile("BlogFiles\\Drafts\\1.xml");
            fileSystem.AddFile("BlogFiles\\Posts\\date_2.xml");
            TestDataStore.ResetCurrentId();
            TestDataStore dataStore = new TestDataStore(fileSystem);
            Post testPost = new Post
            {
                Slug = "Test-slug",
                Title = "Test title",
                Body = "Test body",
                PubDate = DateTime.Now,
                LastModified = DateTime.Now,
                IsPublic = false,
                Excerpt = "Test excerpt"
            };
            dataStore.SavePost(testPost);

            Assert.Equal(3, testPost.Id);
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
                PubDate = DateTimeOffset.Now
            };

            testDataStore.SavePost(oldPost);
            newPost.Id = oldPost.Id;
            testDataStore.UpdatePost(newPost, true);

            Post result = testDataStore.CollectPostInfo($"BlogFiles\\Posts\\{newPost.PubDate.UtcDateTime.ToString("s").Replace(":","-")}_{newPost.Id}.xml");
            Assert.Equal("New-Title", result.Slug);
        }

        [Fact]
        public void CollectPostInfo_EmptyFile_HasPostNode_SetDefaultValues()
        {
            IFileSystem testFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(testFileSystem);

            testFileSystem.WriteFileText($"BlogFiles\\Posts\\empty_file.xml", "<Post/>");
            Post testPost = testDataStore.CollectPostInfo($"BlogFiles\\Posts\\empty_file.xml");

            Assert.NotEqual(0, testPost.Id);
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
