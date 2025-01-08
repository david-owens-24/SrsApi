using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace SrsApi.Managers
{
    /// <summary>
    /// Custom User Manager used to override the base user manager provided via Identity.
    /// Used to automatically assign users to role on creation
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class SrsApiUserManager<TUser> : UserManager<TUser> where TUser : class
    {
        //https://stackoverflow.com/a/78331382
        public SrsApiUserManager(IUserStore<TUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators, IEnumerable<IPasswordValidator<TUser>> passwordValidators, ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<TUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public override async Task<IdentityResult> CreateAsync(TUser user, string password)
        {
            var result = await base.CreateAsync(user, password);

            if (result.Succeeded)
            {
                //TODO: add role here if needed
                //await AddToRoleAsync(user, "User");
            }

            return result;
        }
    }
}
