using BlogTemplate.Data;
using BlogTemplate.Pages.Account;
using BlogTemplate.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlogTemplate.Tests.Fakes;

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

        [Fact]
        public void RegisterUser_CreateFirstUser_LocalRedirect()
        {
            FakeUserManager fakeUserManager = new FakeUserManager();
            fakeUserManager.SetUsers(new List<ApplicationUser>(new ApplicationUser[] {}).AsQueryable());

            RegisterModel registerModel = new RegisterModel(fakeUserManager, null, null, null);
            registerModel.PageContext = new PageContext();

        }
    }
}
