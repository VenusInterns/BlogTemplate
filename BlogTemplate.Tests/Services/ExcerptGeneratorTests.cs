using BlogTemplate.Models;
using BlogTemplate.Services;
using BlogTemplate.Tests.Fakes;
using Xunit;

namespace BlogTemplate.Tests.Services
{
    public class ExcerptGeneratorTests
    {
        [Fact]
        public void CreateExcerpt_BodyLengthExceedsMaxLength_ExcerptIsTruncated()
        {
            BlogDataStore testDataStore = new BlogDataStore(new FakeFileSystem());
            ExcerptGenerator testExcerptGenerator = new ExcerptGenerator();
            string testExcerpt = testExcerptGenerator.CreateExcerpt("This is the body", 5);
            Assert.Equal("This ...", testExcerpt);
        }
    }
}
