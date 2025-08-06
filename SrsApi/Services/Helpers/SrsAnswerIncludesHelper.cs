using SrsApi.DbContext;
using SrsApi.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SrsApi.Services.Helpers
{
    public static class SrsAnswerIncludesHelper
    {
        public static Expression<Func<SrsAnswer, object>>[]? GetIncludes(string includesString)
        {
            if (string.IsNullOrWhiteSpace(includesString))
            {
                return new List<Expression<Func<SrsAnswer, object>>>().ToArray();
            }

            var includes = includesString.Split(",").ToList();
            return GetIncludes(includes);
        }

        public static Expression<Func<SrsAnswer, object>>[]? GetIncludes(List<string> includesStrings)
        {
            if (includesStrings == null)
            {
                return null;
            }

            var includes = new List<Expression<Func<SrsAnswer, object>>>();

            if (includesStrings.Any(x => x == "SearchMethods"))
            {
                includes.Add(x => x.SearchMethods);
                includes.Add(x => x.SearchMethods.Select(y => y.SearchMethod));
            }

            return includes.ToArray();
        }
    }
}