using Core.Infrastructure.Querying;
using System.Linq;

namespace Repository.EF.Infrastructure
{
    public static class QueryTranslator
    {
        public static IQueryable<T> TranslateIntoEFQuery<T>(this Query<T> query, IQueryable<T> efQuery)
        {
            foreach (var predicate in query.Predicates)
                efQuery = efQuery.Where(predicate);

            foreach (var filter in query.KeyValueFilters)
                efQuery = efQuery.Where(filter.Key, filter.Value);

            foreach (var orderByClause in query.OrderByClauses)
            {
                if (orderByClause.Desc)
                {
                    if (!string.IsNullOrWhiteSpace(orderByClause.OrderByPropName))
                        efQuery = efQuery.OrderByDescending(orderByClause.OrderByPropName);
                    else
                        efQuery = efQuery.OrderByDescending(orderByClause.OrderBy);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(orderByClause.OrderByPropName))
                        efQuery = efQuery.OrderBy(orderByClause.OrderByPropName);
                    else
                        efQuery = efQuery.OrderBy(orderByClause.OrderBy);
                }
            }

            return efQuery;
        }
    }
}
