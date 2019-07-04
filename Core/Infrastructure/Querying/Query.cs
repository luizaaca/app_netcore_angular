using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Infrastructure.Querying
{
    public class Query<T>
    {
        public Query() { }
        public Query(Expression<Func<T, bool>> expression)
        {
            Predicates.Add(expression);
        }

        public IList<OrderByClause<T>> OrderByClauses { get; private set; } = new List<OrderByClause<T>>();
        public IList<Expression<Func<T, bool>>> Predicates { get; private set; } = new List<Expression<Func<T, bool>>>();
        public IDictionary<string, object> KeyValueFilters { get; private set; } = new Dictionary<string, object>();

        public Query<T> AddPredicate(Expression<Func<T, bool>> predicate)
        {
            Predicates.Add(predicate);
            return this;
        }
        public Query<T> AddFilter(string key, object value)
        {
            KeyValueFilters.Add(key, value);
            return this;
        }
        public Query<T> OrderBy(Expression<Func<T, object>> orderBy)
        {
            OrderByClauses.Add(new OrderByClause<T>(orderBy, false));
            return this;
        }
        public Query<T> OrderByDescending(Expression<Func<T, object>> orderBy)
        {
            OrderByClauses.Add(new OrderByClause<T>(orderBy, true));
            return this;
        }
        public Query<T> OrderBy(string orderBy)
        {
            OrderByClauses.Add(new OrderByClause<T>(orderBy, false));
            return this;
        }
        public Query<T> OrderByDescending(string orderBy)
        {
            OrderByClauses.Add(new OrderByClause<T>(orderBy, true));
            return this;
        }
    }
}
