using DataTech.System.Versioning.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DataTech.System.Versioning.Extensions
{
    public static class ObjectExtensions
    {
        public static bool EqualsInsensitive(this string value, string target)
        {
            return value?.Equals(target, StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public static bool IsNotEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value) && value.Trim() != string.Empty;
        }

        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) || value.Trim() == string.Empty;
        }


        public static IQueryable<T> OrderBy<T>(this IQueryable<T> objects, Query query)
        {
            if (string.IsNullOrWhiteSpace(query.OrderBy))
                query.OrderBy = "CreationTime";

            //FirstCharToUpper
            query.OrderBy = query.OrderBy.FirstCharToUpper();

            if (!query.OrderBy.Contains("."))
            {
                var props = typeof(T).GetProperties();
                if (!props.Any(p => p.Name.ToLower().Equals(query.OrderBy, StringComparison.OrdinalIgnoreCase)))
                {
                    if (props.Any(p => p.Name.ToLower().Equals("CreationTime".ToLower())))
                    {
                        query.OrderBy = "CreationTime";
                    }
                    else
                    {
                        query.OrderBy =  "Id";
                        
                        if (!props.Any(p => p.Name.ToLower().Equals(query.OrderBy, StringComparison.OrdinalIgnoreCase)))
                        {
                            query.OrderBy = props.FirstOrDefault().Name;
                        }  
                    }
                }
            }

            if (!Enum.IsDefined(typeof(OrderDir), query.OrderDir))
            {
                query.OrderDir = OrderDir.DESC;
            }
            if (query.OrderDir == OrderDir.DESC)
                return objects == null ? null : objects.OrderByDesc($"{query.OrderBy}");
            else
                return objects == null ? null : objects.OrderBy($"{query.OrderBy}");
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource>(
    this IQueryable<TSource> query, string propertyName)
        {
            var entityType = typeof(TSource);

            //Create x=>x.PropName
            var propertyInfo = entityType.GetProperty(propertyName);
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyName);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            //Get System.Linq.Queryable.OrderBy() method.
            var enumarableType = typeof(Queryable);
            var method = enumarableType.GetMethods()
                 .Where(m => m.Name == "OrderBy" && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                     //Put more restriction here to ensure selecting the right overload                
                     return parameters.Count == 2;//overload that has 2 parameters
                 }).Single();
            //The linq's OrderBy<TSource, TKey> has two generic types, which provided here
            MethodInfo genericMethod = method
                 .MakeGenericMethod(entityType, propertyInfo.PropertyType);

            /*Call query.OrderBy(selector), with query and selector: x=> x.PropName
              Note that we pass the selector as Expression to the method and we don't compile it.
              By doing so EF can extract "order by" columns and generate SQL for it.*/
            var newQuery = (IOrderedQueryable<TSource>)genericMethod
                 .Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }
        public static IOrderedQueryable<TSource> OrderByDesc<TSource>(
      this IQueryable<TSource> query, string propertyName)
        {
            var entityType = typeof(TSource);

            //Create x=>x.PropName
            var propertyInfo = entityType.GetProperty(propertyName);
            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyName);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            //Get System.Linq.Queryable.OrderBy() method.
            var enumarableType = typeof(Queryable);
            var method = enumarableType.GetMethods()
                 .Where(m => m.Name == "OrderByDescending" && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                     //Put more restriction here to ensure selecting the right overload                
                     return parameters.Count == 2;//overload that has 2 parameters
                 }).Single();
            //The linq's OrderBy<TSource, TKey> has two generic types, which provided here
            MethodInfo genericMethod = method
                 .MakeGenericMethod(entityType, propertyInfo.PropertyType);

            /*Call query.OrderBy(selector), with query and selector: x=> x.PropName
              Note that we pass the selector as Expression to the method and we don't compile it.
              By doing so EF can extract "order by" columns and generate SQL for it.*/
            var newQuery = (IOrderedQueryable<TSource>)genericMethod
                 .Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }

        public static IEnumerable<T> Page<T>(this IEnumerable<T> objects, int pageSize, int page)
        {
            if (pageSize < 1)
                pageSize = 10;

            if (page < 0)
                page = 0;

            return objects == null ? null : objects.Skip(page * pageSize).Take(pageSize);
        }
        public static IQueryable<T> Page<T>(this IQueryable<T> objects, int pageSize, int page)
        {
            if (pageSize < 1)
                pageSize = 10;

            if (page < 0)
                page = 0;

            return objects == null ? null : objects.Skip(page * pageSize).Take(pageSize);
        }
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input[0].ToString().ToUpper() + input.Substring(1);
            }
        }
    }
}
