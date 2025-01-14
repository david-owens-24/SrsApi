using SrsApi.DbContext;
using SrsApi.DbModels;
using System.Linq.Expressions;

namespace SrsApi.Interfaces
{
    public interface IBaseServiceWithIncludes<T> : IBaseService<T> where T : BaseDbModel
    {
        Expression<Func<T, object>>[]? GetIncludes(string includesStrings);
        Expression<Func<T, object>>[]? GetIncludes(List<string> includesStrings);
    }
}
