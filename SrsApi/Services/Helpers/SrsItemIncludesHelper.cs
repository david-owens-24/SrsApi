using SrsApi.DbContext;
using SrsApi.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SrsApi.Services.Helpers
{
    public static class SrsItemIncludesHelper
    {
        public static Expression<Func<SrsItem, object>>[]? GetIncludes(string includesString)
        {
            if (string.IsNullOrWhiteSpace(includesString))
            {
                return new List<Expression<Func<SrsItem, object>>>().ToArray();
            }

            var includes = includesString.Split(",").ToList();

            return GetIncludes(includes);
        }

        public static Expression<Func<SrsItem, object>>[]? GetIncludes(List<string> includesStrings)
        {
            if (includesStrings == null)
            {
                return null;
            }

            var includes = new List<Expression<Func<SrsItem, object>>>();

            if (includesStrings.Any(x => x == "Answers"))
            {
                includes.Add(x => x.Answers);
            }

            if (includesStrings.Any(x => x == "Level"))
            {
                includes.Add(x => x.Level);
            }

            if (includesStrings.Any(x => x == "Details"))
            {
                includes.Add(x => x.Details);
            }

            return includes.ToArray();
        }
    }
}