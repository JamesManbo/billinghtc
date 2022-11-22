using System;
using System.Linq;
using System.Linq.Expressions;

namespace GenericRepository.Extensions
{
    public static class LinqQueryExtensions
    {
        private static IOrderedQueryable<T> OrderingHelper<T>(IQueryable<T> source, string propertyName, bool asc,
            bool anotherLevel)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), string.Empty);
            MemberExpression property = Expression.PropertyOrField(param, propertyName);
            LambdaExpression sort = Expression.Lambda(property, param);

            MethodCallExpression call = Expression.Call(
                typeof(Queryable),
                (!anotherLevel ? "OrderBy" : "ThenBy") + (!asc ? "Descending" : string.Empty),
                new[] { typeof(T), property.Type },
                source.Expression,
                Expression.Quote(sort));

            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool asc)
        {
            return OrderingHelper(source, propertyName, asc, false);
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName, bool asc)
        {
            return OrderingHelper(source, propertyName, asc, true);
        }

        public static IQueryable<T> Like<T>(this IQueryable<T> source, string propertyName, object propertyValue)
        {
            try
            {
                if (string.IsNullOrEmpty(propertyName) || propertyValue == null) return source;

                var paramExp = Expression.Parameter(typeof(T), "c");
                var propertyExp = Expression.PropertyOrField(paramExp, propertyName);
                var equalMethod = Expression.Equal(propertyExp, Expression.Constant(propertyValue, propertyExp.Type));

                var predicate = Expression.Lambda<Func<T, bool>>(equalMethod, paramExp);
                return source.Where(predicate);
            }
            catch (System.Exception)
            {
                return Enumerable.Empty<T>().AsQueryable();
            }
        }

        public static IQueryable<T> NotLike<T>(this IQueryable<T> source, string propertyName, object propertyValue)
        {
            try
            {
                if (string.IsNullOrEmpty(propertyName) || propertyValue == null) return source;

                var paramExp = Expression.Parameter(typeof(T), "c");
                var propertyExp = Expression.PropertyOrField(paramExp, propertyName);
                var equalMethod =
                    Expression.NotEqual(propertyExp, Expression.Constant(propertyValue, propertyExp.Type));

                var predicate = Expression.Lambda<Func<T, bool>>(equalMethod, paramExp);
                return source.Where(predicate);
            }
            catch (System.Exception)
            {
                return Enumerable.Empty<T>().AsQueryable();
            }
        }
    }
}
