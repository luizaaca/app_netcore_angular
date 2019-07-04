using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Repository.EF.Infrastructure
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> source, string propertyName, object value)
        {
            return source.Where(ToLambda<T>(propertyName, value));
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderBy(ToLambda<T>(propertyName));
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderByDescending(ToLambda<T>(propertyName));
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }

        private static Expression<Func<T, bool>> ToLambda<T>(string propertyName, object value)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);

            ConstantExpression filter;
            Expression typedFilter = null;

            if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                var uType = new NullableConverter(property.Type).UnderlyingType;
                var utValue = Convert.ChangeType(value, uType);
                filter = Expression.Constant(utValue);
                typedFilter = Expression.Convert(filter, property.Type);
            }
            else
            {
                filter = Expression.Constant(value);
                typedFilter = Expression.Convert(filter, property.Type);
            }

            if (property.Type != typeof(string))
                return Expression.Lambda<Func<T, bool>>(Expression.Equal(property, typedFilter), new[] { parameter });

            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            return Expression.Lambda<Func<T, bool>>(Expression.Call(property, method, typedFilter), new[] { parameter });
        }
    }
}
