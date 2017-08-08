using System;
using System.Collections.Generic;
using System.Text;
using BlogTemplate.Models;
using Xunit;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using BlogTemplate.Tests.Fakes;

namespace BlogTemplate.Tests.Pages
{
    class NewTests
    {
        [Fact]
        public void SavePost_CreateExcerpt()
        {
            IFileSystem fakeFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(fakeFileSystem);

            Post newPost = new Post
            {
                Slug = "Title",
                Body = "This is the body of my post",
            };

            testDataStore.SavePost(newPost);

            Assert.True(fakeFileSystem.FileExists($"BlogFiles\\Title.xml"));
            Post result = testDataStore.CollectPostInfo($"BlogFiles\\Title.xml");
            Assert.Equal(result.Body, "This is the body of my post");
            Assert.Equal(result.Excerpt, "This ");
        }
    }
}
