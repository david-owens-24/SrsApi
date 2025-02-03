using Microsoft.EntityFrameworkCore;
using SrsApi.DbContext;
using SrsApi.DbModels;
using SrsApi.Interfaces;
using System.Linq.Expressions;
using System.Linq;
using SrsApi.Enums;
using FuzzySharp;

namespace SrsApi.Services
{  
    public class FuzzyMatchingService : IFuzzyMatchingService
    {
        public bool CheckMatch(string userInput, SrsAnswer answer)
        {
            if (string.IsNullOrEmpty(userInput)) 
            {
                return false;
            }

            foreach(var searchMethod in answer.SearchMethods)
            {
                int ratio = GetFuzzyMatchRatio(userInput, answer.AnswerText, searchMethod.SearchMethod.FuzzySearchMethod);

                if (ratio >= searchMethod.MinumumAcceptedValue)
                {
                    return true;
                }
            }

            return false;
        }

        public int GetFuzzyMatchRatio(string userInput, string compareString, FuzzySearchMethod fuzzySearchMethod)
        {
            int ratio = 0;

            if (string.IsNullOrEmpty(userInput) || string.IsNullOrEmpty(compareString))
            {
                return 0;
            }

            switch (fuzzySearchMethod)
            {
                case FuzzySearchMethod.ExactMatch:
                    if (userInput?.ToLower() == compareString?.ToLower())
                    {
                        ratio = 1;
                    }
                    break;
                case FuzzySearchMethod.SimpleRatio:
                    ratio = Fuzz.Ratio(userInput, compareString);
                    break;
                case FuzzySearchMethod.PartialRatio:
                    ratio = Fuzz.PartialRatio(userInput, compareString);
                    break;
                case FuzzySearchMethod.TokenSortRatio:
                    ratio = Fuzz.TokenSortRatio(userInput, compareString);
                    break;
                case FuzzySearchMethod.PartialTokenSortRatio:
                    ratio = Fuzz.PartialTokenSortRatio(userInput, compareString);
                    break;
                case FuzzySearchMethod.TokenSetRatio:
                    ratio = Fuzz.TokenSetRatio(userInput, compareString);
                    break;
                case FuzzySearchMethod.PartialTokenSetRatio:
                    ratio = Fuzz.PartialTokenSetRatio(userInput, compareString);
                    break;
                case FuzzySearchMethod.TokenInitialismRatio:
                    ratio = Fuzz.TokenInitialismRatio(userInput, compareString);
                    break;
                case FuzzySearchMethod.PartialTokenInitialismRatio:
                    ratio = Fuzz.PartialTokenInitialismRatio(userInput, compareString);
                    break;
                case FuzzySearchMethod.TokenAbbreviationRatio:
                    ratio = Fuzz.TokenAbbreviationRatio(userInput, compareString);
                    break;
                case FuzzySearchMethod.PartialTokenAbbreviationRatio:
                    ratio = Fuzz.PartialTokenAbbreviationRatio(userInput, compareString);
                    break;
                case FuzzySearchMethod.WeightedRatio:
                    ratio = Fuzz.WeightedRatio(userInput, compareString);
                    break;
            }

            return ratio;
        }
    }
}
