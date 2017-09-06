using BlogTemplate._1.Data;
using BlogTemplate._1.Pages.Account;
using BlogTemplate._1.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;
using Microsoft.AspNetCore.Identity;

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
            Assert.IsType(typeof(RedirectToPageResult), result); 
        }

        [Fact]
        public void RegisterUser_FirstUser_LocalRedirect()
        {
            // Arrange
            FakeUserManager fakeUserManager = new FakeUserManager();
            fakeUserManager.SetUsers(new List<ApplicationUser>(new ApplicationUser[] {}).AsQueryable());

            FakeSignInManager fakeSignInManager = new FakeSignInManager();

            RegisterModel registerModel = new RegisterModel(fakeUserManager, fakeSignInManager, new FakeLogger<LoginModel>(), null);
            registerModel.PageContext = new PageContext();

            registerModel.Input = new RegisterModel.InputModel
            {
                Email = "test@test.com",
                Password = "TestPassword.1",
            };

            // Act
            IActionResult result = registerModel.OnPostAsync().Result;

            // Assert
            Assert.IsType(typeof(LocalRedirectResult), result);
        }
    }
}
