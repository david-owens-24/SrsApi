using SrsApi.DbContext;
using SrsApi.DbModels;
using System.Linq.Expressions;

namespace SrsApi.Interfaces
{
    public interface IErrorService
    {
        Task<Error> LogError(string error);
        Task<Error> LogError(Exception exception);
    }
}
