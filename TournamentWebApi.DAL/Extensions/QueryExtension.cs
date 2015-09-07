using System;
using System.Linq;
using System.Linq.Expressions;
using TournamentWebApi.Core.Enums;

namespace TournamentWebApi.DAL.Extensions
{
    public static class QueryExtension
    {
        public static IQueryable<TEntity> FilterBy<TEntity>(
            this IQueryable<TEntity> source,
            Expression<Func<TEntity, bool>> filterCondition)
        {
            IQueryable<TEntity> result = source;

            if (filterCondition != null)
            {
                result = result.Where(filterCondition);
            }

            return result;
        }

        public static IQueryable<TEntity> SortBy<TEntity>(
            this IQueryable<TEntity> source,
            Expression<Func<TEntity, object>> sortCondition,
            SortDirection sortDirection)
        {
            IQueryable<TEntity> result = source;

            if (sortCondition != null)
            {
                result = (sortDirection == SortDirection.Ascending)
                    ? result.OrderBy(sortCondition)
                    : result.OrderByDescending(sortCondition);
            }

            return result;
        }
    }
}