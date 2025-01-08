using SrsApi.DbContext;

namespace SrsApi.Interfaces
{
    public interface IUserResolutionService
    {
        string GetCurrentSessionUserId(ApplicationDbContext dbContext);
    }
}
