using BlogTemplate.Models;
using BlogTemplate.Services;
using BlogTemplate.Tests.Fakes;
using Xunit;

namespace BlogTemplate.Tests.Services
{
    public class SlugGeneratorTests
    {
        [Fact]
        public void CreateSlug_ReplacesSpaces()
        {
            IFileSystem testFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(testFileSystem);
            SlugGenerator testSlugGenerator = new SlugGenerator(testDataStore);
            Post test = new Post
            {
                Title = "Test title"
            };
            test.Slug = testSlugGenerator.CreateSlug(test.Title);

            Assert.Equal(test.Slug, "Test-title");
        }

        [Fact]
        public void CreateSlug_TitleContainsInvalidChars_RemoveInvalidCharsInSlug()
        {
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());
            SlugGenerator testSlugGenerator = new SlugGenerator(testDataStore);
            string slug1 = testSlugGenerator.CreateSlug("test?");
            string slug2 = testSlugGenerator.CreateSlug("test<");
            string slug3 = testSlugGenerator.CreateSlug("test>");
            string slug4 = testSlugGenerator.CreateSlug("test/");
            string slug5 = testSlugGenerator.CreateSlug("test&");
            string slug6 = testSlugGenerator.CreateSlug("test!");
            Assert.Equal("test", slug1);
            Assert.Equal("test", slug2);
            Assert.Equal("test", slug3);
            Assert.Equal("test", slug4);
            Assert.Equal("test", slug5);
            Assert.Equal("test", slug6);
        }

    }
}
