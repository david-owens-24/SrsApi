using Microsoft.EntityFrameworkCore;
using SrsApi.DbContext;
using SrsApi.DbModels;
using SrsApi.Interfaces;
using System.Linq.Expressions;
using System.Linq;

namespace SrsApi.Services
{
    //Adapted from https://robdotnet.medium.com/effortlessly-create-crud-apis-in-c-bcc759691a91

    public class SrsItemService : BaseService<SrsItem>, ISrsItemService
    {
        protected ApplicationDbContext _context;
        protected DbSet<SrsItem> _dbSet;

        public SrsItemService(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<SrsItem>();
        }

        public Expression<Func<SrsItem, object>>[]? GetIncludes(string includesString)
        {
            if (string.IsNullOrWhiteSpace(includesString))
            {
                return null;
            }

            var includes = includesString.Split(",").ToList();

            return GetIncludes(includes);
        }

        public Expression<Func<SrsItem, object>>[]? GetIncludes(List<string> includesStrings)
        {
            if(includesStrings == null)
            {
                return null;
            }

            var includes = new List<Expression<Func<SrsItem, object>>>();

            if(includesStrings.Any(x=>x == "Answers"))
            {
                includes.Add(x => x.Answers);
            }

            if (includesStrings.Any(x => x == "Level"))
            {
                includes.Add(x => x.Level);
            }

            return includes.ToArray();
        }


        /// <summary>
        /// Add a new SrsItem to the database.
        /// If the Order is set to 0, then it is added at the end.
        /// If the Order is not set to 0, then it is inserted into the correct position, and all existing SrsItems with an Order greater than the new value are pushed down one.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<SrsItem> Add(SrsItem entity)
        {
            var currentHighestOrder = _dbSet.Where(x => x.Deleted == null && x.Level.Id == entity.Level.Id).Max(x => x.Order);

            //determine if we can just add this new SrsItem at the end
            //(i.e. Order unset, Order equals current max order, or the Order is greater than any existing order)
            if (entity.Order == 0 || entity.Order == currentHighestOrder || entity.Order > currentHighestOrder)
            {
                entity.Order = currentHighestOrder + 1;
            }
            else
            {                
                FixOrder(currentHighestOrder, entity.Level.Id);
            }

            var addedEntity = (await _context.AddAsync(entity)).Entity;

            await _context.SaveChangesAsync();

            return addedEntity;
        }

        //don't need to change the Order here, as the SrsItems will still be in order even if a specific value for Order is missing
        //public void Delete(Guid UID)
        //{
        //    var entity = _dbSet.FirstOrDefault(x => x.UID == UID);

        //    if (entity != null)
        //    {
        //        entity.Deleted = DateTime.Now;
        //    }            
        //}
               

        public async Task<SrsItem> Update(SrsItem entity)
        {
            var existingDbItem = _dbSet.Include(x=>x.Level).First(x=>x.UID == entity.UID);

            if (existingDbItem.Order == entity.Order && existingDbItem.Level == entity.Level)
            {
                //no change in Order or Level so no need to shuffle anything, can just save the update to the database 
                return _context.Update(entity).Entity;
            }

            var currentHighestOrder = _dbSet.Where(x => x.Deleted == null && x.Level.Id == entity.Level.Id).Max(x => x.Order);

            if (entity.Order == 0 || entity.Order == currentHighestOrder || entity.Order > currentHighestOrder)
            {
                entity.Order = currentHighestOrder + 1;
            }
            else
            {
                FixOrder(currentHighestOrder, entity.Level.Id, new List<int> { entity.Id });
            }

            var updatedEntity = _context.Update(entity).Entity;

            return updatedEntity;
        }

        /// <summary>
        /// Pushes all SrsItems after the currentHighestOrder down an Order. Should be called when adding a new SrsItem when there is already an SrsItem with the same Order, 
        /// or when updating the order of an existing SrsItem.
        /// </summary>
        /// <param name="currentHighestOrder">The current highest order in the provided SrsItemLevel</param>
        /// <param name="srsItemLevelId">The Id of the SrsItemLevel whose order will be fixed.</param>
        /// <param name="skip">A list of Ids to skip when pushing down an order. Used to prevent SrsItems from being pushed down a level on an update.</param>
        private void FixOrder(int currentHighestOrder, int srsItemLevelId, List<int> skip = null)
        {
            var srsItemsToUpdate = _dbSet.Where(x => x.Deleted == null && x.Order >= currentHighestOrder && x.Level.Id == srsItemLevelId).OrderBy(x => x.Order);

            foreach (var item in srsItemsToUpdate)
            {
                item.Order += 1;
            }
        }


    }
}
