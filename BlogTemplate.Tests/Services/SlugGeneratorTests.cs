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
    }
}
