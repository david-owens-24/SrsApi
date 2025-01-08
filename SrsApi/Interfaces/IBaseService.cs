using SrsApi.DbModels;
using System.Linq.Expressions;

namespace SrsApi.Interfaces
{
    public interface IBaseService<T> where T : BaseDbModel
    {
        void SaveChanges();
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        void Delete(Guid UID);
        Task<T>? GetByUID(Guid UID, Expression<Func<T, object>>[]? includes = null, bool includeDeleted = false);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int? skip = null, int? take = null, bool includeDeleted = false);
    }
}
