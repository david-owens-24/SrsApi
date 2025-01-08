using SrsApi.DbContext;
using SrsApi.Interfaces;

namespace SrsApi.Services
{
    //https://stackoverflow.com/a/53142427

    public class UserResolutionService : IUserResolutionService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserResolutionService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentSessionUserId(ApplicationDbContext dbContext)
        {
            var currentSessionUserEmail = httpContextAccessor.HttpContext.User.Identity.Name;

            var user = dbContext.Users.First(u => u.Email == (currentSessionUserEmail));

            return user.Id;
        }
    }
}
