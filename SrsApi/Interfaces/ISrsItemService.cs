using SrsApi.DbContext;
using SrsApi.DbModels;
using System.Linq.Expressions;

namespace SrsApi.Interfaces
{
    public interface ISrsItemService : IBaseService<SrsItem>
    {
        Expression<Func<SrsItem, object>>[]? GetIncludes(string includesStrings);
        Expression<Func<SrsItem, object>>[]? GetIncludes(List<string> includesStrings);
    }
}
