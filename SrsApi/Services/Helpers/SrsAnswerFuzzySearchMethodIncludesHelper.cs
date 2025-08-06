using SrsApi.DbContext;
using SrsApi.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SrsApi.Services.Helpers
{
    public static class SrsAnswerFuzzySearchMethodIncludesHelper
    {
        public static Expression<Func<SrsAnswerFuzzySearchMethod, object>>[]? GetIncludes(string includesString)
        {
            if (string.IsNullOrWhiteSpace(includesString))
            {
                return new List<Expression<Func<SrsAnswerFuzzySearchMethod, object>>>().ToArray();
            }

            var includes = includesString.Split(",").ToList();
            return GetIncludes(includes);
        }

        public static Expression<Func<SrsAnswerFuzzySearchMethod, object>>[]? GetIncludes(List<string> includesStrings)
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