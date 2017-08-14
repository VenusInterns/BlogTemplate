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
