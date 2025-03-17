using Microsoft.EntityFrameworkCore;
using SrsApi.DbContext;
using SrsApi.DbModels;
using SrsApi.Interfaces;
using System.Linq.Expressions;
using System.Linq;

namespace SrsApi.Services
{
    //Adapted from https://robdotnet.medium.com/effortlessly-create-crud-apis-in-c-bcc759691a91

    public class BaseService<T> : IBaseService<T> where T : BaseDbModel
    {
        protected ApplicationDbContext _context;
        protected DbSet<T> _dbSet;

        public BaseService(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task<T> Add(T entity)
        {
            var addedEntity = (await _context.AddAsync(entity)).Entity;

            await _context.SaveChangesAsync();

            return addedEntity;
        }

        public void Delete(Guid UID)
        {
            var entity = _dbSet.FirstOrDefault(x => x.UID == UID);

            if (entity != null)
            {
                entity.Deleted = DateTime.Now;
            }
        }

        public void Delete(Guid UID, DateTime deletedDateTime)
        {
            var entity = _dbSet.FirstOrDefault(x => x.UID == UID);

            if (entity != null)
            {
                entity.Deleted = deletedDateTime;
            }
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int? skip = null, int? take = null, bool includeDeleted = false)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!includeDeleted)
            {
                query = query.Where(x => x.Deleted == null);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<T>? GetByUID(Guid UID, Expression<Func<T, object>>[]? includes = null, bool includeDeleted = false)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            query = query.Where(x => x.UID == UID);

            if (!includeDeleted)
            {
                query = query.Where(x => x.Deleted == null);
            }

            return await query.FirstOrDefaultAsync();

        }

        public async Task<T> Update(T entity)
        {
            var updatedEntity = _context.Update(entity).Entity;

            return updatedEntity;
        }
    }
}
