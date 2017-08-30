using BlogTemplate._1.Models;
using BlogTemplate._1.Pages;
using BlogTemplate._1.Tests.Fakes;
using BlogTemplate._1.Services;
using Xunit;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogTemplate._1.Tests.Pages
{
    public class NewTests
    {
        [Fact]
        public void SavePost_NoExcerptIsEntered_AutoGenerateExcerpt()
        {
            IFileSystem fakeFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(fakeFileSystem);
            ExcerptGenerator testExcerptGenerator = new ExcerptGenerator();
            testExcerptGenerator.maxLength = 5;
            SlugGenerator testSlugGenerator = new SlugGenerator(testDataStore);

            NewModel model = new NewModel(testDataStore, testSlugGenerator, testExcerptGenerator);
            model.PageContext = new PageContext();
            model.OnGet();
            model.NewPost.Title = "Title";
            model.NewPost.Body = "This is the body";

            model.OnPostPublish();

            Assert.Equal("This is the body", model.NewPost.Body);
            Assert.Equal("This ...", model.NewPost.Excerpt);
        }
    }
}
