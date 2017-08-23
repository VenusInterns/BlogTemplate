using BlogTemplate.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlogTemplate.Tests.Fakes
{
    class FakeUserManager : UserManager<ApplicationUser>
    {

        public FakeUserManager()
            : this(new FakeUserStore(), null, null, null, null, null, null, null, null)
        {
        }

        public FakeUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        private IQueryable<ApplicationUser> _users = new List<ApplicationUser>().AsQueryable();
        public void SetUsers(IQueryable<ApplicationUser> users)
        {
            _users = users;
        }

        public override IQueryable<ApplicationUser> Users
        {
            get
            {
                return _users;
            }
        }

        #region Internal dependencies
        class FakeUserStore : IUserStore<ApplicationUser>
        {
            public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
        #endregion
    }
}
