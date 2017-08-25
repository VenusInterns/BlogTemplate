using BlogTemplate._1.Data;
using BlogTemplate._1.Pages.Account;
using BlogTemplate.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

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
            Assert.Equal(typeof(RedirectToPageResult), result.GetType()); 
        }
    }
}
