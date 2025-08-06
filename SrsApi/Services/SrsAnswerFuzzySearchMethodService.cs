using Microsoft.EntityFrameworkCore;
using SrsApi.DbContext;
using SrsApi.DbModels;
using SrsApi.Interfaces;
using System.Linq.Expressions;
using System.Linq;
using SrsApi.Services.Helpers;

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
            return SrsAnswerFuzzySearchMethodIncludesHelper.GetIncludes(includesString);
        }

        public Expression<Func<SrsAnswerFuzzySearchMethod, object>>[]? GetIncludes(List<string> includesStrings)
        {
            return SrsAnswerFuzzySearchMethodIncludesHelper.GetIncludes(includesStrings);
        }
    }
}
