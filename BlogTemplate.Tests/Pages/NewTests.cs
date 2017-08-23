using BlogTemplate._1.Models;
using BlogTemplate._1.Tests.Fakes;
using Xunit;

namespace BlogTemplate._1.Tests.Pages
{
    class NewTests
    {
        [Fact]
        public void SavePost_NoExcerptIsEntered_AutoGenerateExcerpt()
        {
            IFileSystem fakeFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(fakeFileSystem);

            Post newPost = new Post
            {
                Title = "Title",
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
