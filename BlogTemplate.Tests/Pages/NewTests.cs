using BlogTemplate._1.Models;
using BlogTemplate._1.Pages;
using BlogTemplate._1.Services;
using BlogTemplate._1.Tests.Fakes;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Xunit;

namespace BlogTemplate._1.Tests.Pages
{
    public class NewTests
    {
        [Fact]
        public void OnPostPublish_NoExcerptIsEntered_AutoGenerateExcerpt()
        {
            IFileSystem fakeFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(fakeFileSystem);
            ExcerptGenerator testExcerptGenerator = new ExcerptGenerator(5);
            SlugGenerator testSlugGenerator = new SlugGenerator(testDataStore);

            NewModel model = new NewModel(testDataStore, testSlugGenerator, testExcerptGenerator);
            model.PageContext = new PageContext();
            model.NewPost = new NewModel.NewPostViewModel {
                Title = "Title",
                Body = "This is the body",
            };

            model.OnPostPublish();

            Assert.Equal("This is the body", model.NewPost.Body);
            Assert.Equal("This ...", model.NewPost.Excerpt);
        }

        [Fact]
        public void OnPostSaveDraft_NoExcerptIsEntered_AutoGenerateExcerpt()
        {
            IFileSystem fakeFileSystem = new FakeFileSystem();
            BlogDataStore testDataStore = new BlogDataStore(fakeFileSystem);
            ExcerptGenerator testExcerptGenerator = new ExcerptGenerator(5);
            SlugGenerator testSlugGenerator = new SlugGenerator(testDataStore);

            NewModel model = new NewModel(testDataStore, testSlugGenerator, testExcerptGenerator);
            model.PageContext = new PageContext();
            model.NewPost = new NewModel.NewPostViewModel {
                Title = "Title",
                Body = "This is the body",
            };

            model.OnPostSaveDraft();

            Assert.Equal("This is the body", model.NewPost.Body);
            Assert.Equal("This ...", model.NewPost.Excerpt);
        }
    }
}
