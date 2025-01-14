using Microsoft.EntityFrameworkCore;
using SrsApi.DbContext;
using SrsApi.DbModels;
using SrsApi.Interfaces;
using System.Linq.Expressions;
using System.Linq;
using SrsApi.Enums;

namespace SrsApi.Services
{
    //Adapted from https://robdotnet.medium.com/effortlessly-create-crud-apis-in-c-bcc759691a91

    public class FuzzySearchMethodService : IFuzzySearchMethodService
    {
        protected ApplicationDbContext _context;
        protected DbSet<SrsFuzzySearchMethod> _dbSet;

        public FuzzySearchMethodService(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<SrsFuzzySearchMethod>();
        }

        public async Task<SrsFuzzySearchMethod>? Get(FuzzySearchMethod fuzzySearchMethod)
        {
            return await _dbSet.FirstAsync(x=>x.FuzzySearchMethod==fuzzySearchMethod);
        }
    }
}
