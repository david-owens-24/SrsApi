using Microsoft.EntityFrameworkCore;
using SrsApi.DbContext;
using SrsApi.DbModels;
using SrsApi.Interfaces;
using System.Linq.Expressions;
using System.Linq;

namespace SrsApi.Services
{
    //Adapted from https://robdotnet.medium.com/effortlessly-create-crud-apis-in-c-bcc759691a91

    public class SrsAnswerService : BaseService<SrsAnswer>, IBaseServiceWithIncludes<SrsAnswer>
    {
        protected ApplicationDbContext _context;
        protected DbSet<SrsAnswer> _dbSet;

        public SrsAnswerService(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<SrsAnswer>();
        }

        public Expression<Func<SrsAnswer, object>>[]? GetIncludes(string includesString)
        {
            if (string.IsNullOrWhiteSpace(includesString))
            {
                return null;
            }

            var includes = includesString.Split(",").ToList();

            return GetIncludes(includes);
        }

        public Expression<Func<SrsAnswer, object>>[]? GetIncludes(List<string> includesStrings)
        {
            if(includesStrings == null)
            {
                return null;
            }

            var includes = new List<Expression<Func<SrsAnswer, object>>>();

            if(includesStrings.Any(x=>x == "SearchMethods"))
            {
                includes.Add(x => x.SearchMethods);
                includes.Add(x => x.SearchMethods.Select(y=>y.SearchMethod));
            }

            return includes.ToArray();
        }
    }
}
