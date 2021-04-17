using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace BPWA.Common.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            if (!condition)
                return query;

            return query.Where(predicate);
        }

        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> query, bool condition, Func<T, bool> predicate)
        {
            if (!condition)
                return query;

            return query.Where(predicate);
        }

        public static IQueryable<TEntity> IncludeIf<TEntity, TProperty>([NotNullAttribute] this IQueryable<TEntity> source, bool condition, [NotNullAttribute] Expression<Func<TEntity, TProperty>> navigationPropertyPath) where TEntity : class
        {
            if (!condition)
                return source;

            return source.Include(navigationPropertyPath);
        }
    }
}
