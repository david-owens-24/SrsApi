using Microsoft.EntityFrameworkCore;
using SrsApi.DbContext;
using SrsApi.DbModels;
using SrsApi.Interfaces;
using System.Linq.Expressions;
using System.Linq;

namespace SrsApi.Services
{
    //Adapted from https://robdotnet.medium.com/effortlessly-create-crud-apis-in-c-bcc759691a91

    public class SrsAnswerFuzzySearchMethodService : BaseService<SrsAnswerFuzzySearchMethod>, IBaseServiceWithIncludes<SrsAnswerFuzzySearchMethod>
    {
        protected ApplicationDbContext _context;
        protected DbSet<SrsAnswerFuzzySearchMethod> _dbSet;

        public SrsAnswerFuzzySearchMethodService(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<SrsAnswerFuzzySearchMethod>();
        }

        public Expression<Func<SrsAnswerFuzzySearchMethod, object>>[]? GetIncludes(string includesString)
        {
            if (string.IsNullOrWhiteSpace(includesString))
            {
                return null;
            }

            var includes = includesString.Split(",").ToList();

            return GetIncludes(includes);
        }

        public Expression<Func<SrsAnswerFuzzySearchMethod, object>>[]? GetIncludes(List<string> includesStrings)
        {
            if (includesStrings == null)
            {
                return null;
            }

            var includes = new List<Expression<Func<SrsAnswerFuzzySearchMethod, object>>>();

            if (includesStrings.Any(x => x == "SearchMethod"))
            {
                includes.Add(x => x.SearchMethod);
            }

            return includes.ToArray();
        }
    }
}
