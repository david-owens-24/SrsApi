using SrsApi.DbContext;
using SrsApi.DbModels;
using SrsApi.Enums;
using System.Linq.Expressions;

namespace SrsApi.Interfaces
{
    public interface IFuzzySearchMethodService
    {
        Task<SrsFuzzySearchMethod>? Get(FuzzySearchMethod fuzzySearchMethod);        
    }
}
