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
            return SrsAnswerIncludesHelper.GetIncludes(includesString);
        }

        public Expression<Func<SrsAnswer, object>>[]? GetIncludes(List<string> includesStrings)
        {
            return SrsAnswerIncludesHelper.GetIncludes(includesStrings);
        }
    }
}
