using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlogTemplate._1.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlogTemplate._1.Tests.Fakes
{
    class FakeSignInManager : SignInManager<ApplicationUser>
    {
        public FakeSignInManager()
            : this(new FakeUserManager(), null, null, null, null, null)
        {
        }

        public FakeSignInManager(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<ApplicationUser>> logger, IAuthenticationSchemeProvider schemes)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {
        }

        public override Task SignInAsync(ApplicationUser user, bool isPersistent, string authenticationMethod = null)
        {
            return Task.FromResult(IdentityResult.Success);
        }

    }

}

