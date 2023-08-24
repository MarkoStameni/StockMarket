using System.Linq.Expressions;
using System.Reflection;

namespace StockMarket.Server.Helpers
{
    public static class EfQueryableExtensions
    {
        public static (IQueryable<T>, int) Paginate<T>(this IQueryable<T> query, int totalRecords, int page, int recordCount)
        {
            var totalPages = 1;

            if (recordCount > 0)
            {
                totalPages = (int)Math.Ceiling((decimal)totalRecords / recordCount);
                var offset = (page - 1) * recordCount;

                query = query.Skip(offset).Take(recordCount);
            }

            return (query, totalPages);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string name, string direction = "asc")
        {
            var propInfo = GetPropertyInfo(typeof(T), name);
            var expr = GetOrderExpression(typeof(T), name);
            var methodName = (direction.ToLowerInvariant() == "asc") ? "OrderBy" : "OrderByDescending";

            var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == methodName && m.GetParameters().Length == 2);
            var genericMethod = method?.MakeGenericMethod(typeof(T), propInfo.PropertyType);

            if (genericMethod != null)
            {
                var result = genericMethod.Invoke(null, new object[] { query, expr });

                if (result != null)
                {
                    return (IQueryable<T>)result;
                }
            }

            throw new ArgumentException("Could not order the query with the specified column");
        }

        private static PropertyInfo GetPropertyInfo(Type objType, string propertyNames)
        {
            var splitPropertyNames = propertyNames.Split('.');
            var currentType = objType;

            // We want to fetch the bottom most property since we are using its type when making a generic method out of the order by
            foreach (var splitPropertyName in splitPropertyNames)
            {
                var properties = currentType.GetProperties();
                var property = properties.First(p => p.Name.ToLowerInvariant() == splitPropertyName.ToLowerInvariant());

                if (property != null)
                {
                    currentType = property.PropertyType;
                    if (splitPropertyName == splitPropertyNames.Last())
                    {
                        return property;
                    }
                }
            }

            throw new ArgumentException($"Could not get column named '{propertyNames}'");
        }

        private static LambdaExpression GetOrderExpression(Type type, string propertyName)
        {
            var param = Expression.Parameter(type, "x");

            Expression body = param;

            // Here we want to create and expression resembling " x.Child.Grandchild.Id " for use when ordering ef core entities
            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }

            return Expression.Lambda(body, param);
        }
    }
}
