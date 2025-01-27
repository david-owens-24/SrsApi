using Microsoft.EntityFrameworkCore;
using SrsApi.DbContext;
using SrsApi.DbModels;
using SrsApi.Interfaces;
using System.Linq.Expressions;
using System.Linq;

namespace SrsApi.Services
{
    //Adapted from https://robdotnet.medium.com/effortlessly-create-crud-apis-in-c-bcc759691a91

    public class ErrorService : IErrorService
    {
        protected ApplicationDbContext _context;
        protected DbSet<Error> _dbSet;

        public ErrorService(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Error>();
        }

        public async Task<Error> LogError(string error)
        {
            Error newError = new Error
            {
                ExceptionText = error,
            };

            var addedEntity = (await _context.AddAsync(newError)).Entity;

            await _context.SaveChangesAsync();

            return addedEntity;
        }

        public async Task<Error> LogError(Exception exception)
        {
            return await LogError(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }
    }
}
