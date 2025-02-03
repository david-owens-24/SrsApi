using SrsApi.DbContext;
using SrsApi.Enums;

namespace SrsApi.Interfaces
{
    public interface IFuzzyMatchingService
    {
        bool CheckMatch(string userInput, SrsAnswer answer);
        int GetFuzzyMatchRatio(string userInput, string compareString, FuzzySearchMethod fuzzySearchMethod);
    }
}
