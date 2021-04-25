using System;
using System.Linq;
using System.Linq.Expressions;

namespace BPWA.Common.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool? condition, Expression<Func<T, bool>> predicate)
        {
            if (!condition.GetValueOrDefault())
                return query;

            return query.Where(predicate);
        }
    }
}
