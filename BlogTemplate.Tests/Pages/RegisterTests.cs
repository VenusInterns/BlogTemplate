using BlogTemplate.Data;
using BlogTemplate.Pages.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BlogTemplate.Tests.Services
{
    class SingleUserRegistrationTests
    {
        [Fact]
        public void RegisterUser_CreateSecondUser_RegistrationFails()
        {
            // Arrange
            //SignInManager<ApplicationUser> fakeSignInManager = new SignInManager<ApplicationUser>();
            //UserManager<ApplicationUser> fakeUserManager = new UserManager<ApplicationUser>();
            //ILogger<LoginModel> fakeLogger = new ILogger<LoginModel>();

            RegisterModel registerModel = new RegisterModel(null, null, null, null);

            // Act

            IActionResult result = registerModel.OnPostAsync().Result;

            // Assert
            Assert.False(string.IsNullOrEmpty((string)registerModel.ViewData["UserAlreadyRegisteredMessage"]));
            Assert.Equal(typeof(PageResult), result.GetType());
        }
    }
}
