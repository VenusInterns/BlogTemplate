using BlogTemplate._1.Data;
using BlogTemplate._1.Pages.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using BlogTemplate.Tests.Fakes;
using System.Threading;

namespace BlogTemplate._1.Tests.Pages.Account
{
    public class RegisterTests
    {

        [Fact]
        public void RegisterUser_CreateSecondUser_RedirectToUserAlreadyExistsPage()
        {
            // Arrange
            FakeUserManager fakeUserManager = new FakeUserManager();
            fakeUserManager.SetUsers(new List<ApplicationUser>(new ApplicationUser[] {
                        new ApplicationUser
                        {
                            UserName = "Test User 1",
                        }
                    }).AsQueryable());

            RegisterModel registerModel = new RegisterModel(fakeUserManager, null, null, null);
            registerModel.PageContext = new PageContext();

            // Act
            IActionResult result = registerModel.OnPostAsync().Result;

            // Assert
            Assert.Equal(typeof(RedirectToPageResult), result.GetType()); //True if result is a redirection to a page (AlreadyRegistered page)
        }
    }
}
