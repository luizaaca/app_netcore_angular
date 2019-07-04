using System;
using System.Linq.Expressions;

namespace Core.Infrastructure.Querying
{
    public class OrderByClause<T>
    {
        internal OrderByClause(Expression<Func<T, object>> orderBy, bool desc)
        {
            OrderBy = orderBy;
            Desc = desc;
        }
        internal OrderByClause(string orderBy, bool desc)
        {
            OrderByPropName = orderBy;
            Desc = desc;
        }

        public bool Desc { get; private set; }
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public string OrderByPropName { get; private set; }
    }
}
