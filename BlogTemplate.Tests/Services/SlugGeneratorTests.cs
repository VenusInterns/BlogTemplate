using BlogTemplate.Models;
using BlogTemplate.Services;
using BlogTemplate.Tests.Fakes;
using Xunit;

namespace BlogTemplate.Tests.Services
{
    public class SlugGeneratorTests
    {
        [Fact]
        public void CreateSlug_SlugIsUnique()
        {
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());
            SlugGenerator testSlugGenerator = new SlugGenerator(testDataStore);

            string slug = testSlugGenerator.CreateSlug("Test Title");

            Assert.Equal("Test-Title", slug);
        }

        [Fact]
        public void CreateSlug_SlugExists_AppendNumber()
        {
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());
            testDataStore.SavePost(new Post { Slug = "test" });
            SlugGenerator testSlugGenerator = new SlugGenerator(testDataStore);

            string slug = testSlugGenerator.CreateSlug("test");

            Assert.Equal("test-1", slug);
        }

        [Fact]
        public void CreateSlug_SlugExists_MultipleConflicts()
        {
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());
            testDataStore.SavePost(new Post { Slug = "test" });
            testDataStore.SavePost(new Post { Slug = "test-1" });
            SlugGenerator testSlugGenerator = new SlugGenerator(testDataStore);

            string slug = testSlugGenerator.CreateSlug("test");

            Assert.Equal("test-2", slug);
        }

    }
}

