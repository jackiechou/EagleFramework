using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Eagle.Common.Extensions.Searching
{
    public class ExpressionBuilder
    {
        /*
         * How to use Filter:
         * 
         * List<Filter> filter = new List<Filter>()
            {
                new Filter { PropertyName = "MailServerProviderName" ,
                    Operation = Op .StartsWith, Value = "M"  },
                new Filter { PropertyName = "MailServerProviderName" ,
                    Operation = Op .EndsWith, Value = "L"  }
            };

            var deleg = ExpressionBuilder.GetExpression<MailServerProvider>(filter).Compile();
            var filteredCollection = queryable.Where(deleg).ApplyPaging(page.Value, pageSize.Value);

            return filteredCollection.AsEnumerable();
         */

        private static MethodInfo containsMethod = typeof(string).GetMethod("Contains");
        private static MethodInfo startsWithMethod =
        typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        private static MethodInfo endsWithMethod =
        typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });


        public static Expression<Func<T,
        bool>> GetExpression<T>(IList<Filter> filters, bool isSearching = false)
        {
            if (filters.Count == 0)
                return null;

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression exp = null;

            if (filters.Count == 1)
                exp = GetExpression<T>(param, filters[0]);
            else if (filters.Count == 2)
                exp = GetExpression<T>(param, filters[0], filters[1], isSearching);
            else
            {
                while (filters.Count > 0)
                {
                    var f1 = filters[0];
                    var f2 = filters[1];

                    if (exp == null)
                        exp = GetExpression<T>(param, filters[0], filters[1], isSearching);
                    else
                    {
                        if (isSearching)
                            exp = Expression.Or(exp, GetExpression<T>(param, filters[0], filters[1], isSearching));
                        else
                            exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0], filters[1], isSearching));
                    }
                    filters.Remove(f1);
                    filters.Remove(f2);

                    if (filters.Count == 1)
                    {
                        if (isSearching)
                            exp = Expression.Or(exp, GetExpression<T>(param, filters[0]));
                        else
                            exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0]));
                        filters.RemoveAt(0);
                    }
                }
            }

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }

        private static Expression GetExpression<T>(ParameterExpression param, Filter filter)
        {
            MemberExpression member = Expression.Property(param, filter.PropertyName);
            ConstantExpression constant = Expression.Constant(filter.Value);
            StringComparison comparison = StringComparison.InvariantCultureIgnoreCase;

            switch (filter.Operation)
            {
                case Op.Equals:
                    return Expression.Equal(member, constant);

                case Op.GreaterThan:
                    return Expression.GreaterThan(member, constant);

                case Op.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);

                case Op.LessThan:
                    return Expression.LessThan(member, constant);

                case Op.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, constant);

                case Op.Contains:
                    var indexOf = Expression.Call(member, "IndexOf", null,
                        Expression.Constant(filter.Value, typeof(string)),
                        Expression.Constant(comparison));
                    return Expression.GreaterThanOrEqual(indexOf, Expression.Constant(0));

                case Op.StartsWith:
                    return Expression.Call(member, startsWithMethod, constant);

                case Op.EndsWith:
                    return Expression.Call(member, endsWithMethod, constant);
            }

            return null;
        }

        private static BinaryExpression GetExpression<T>
        (ParameterExpression param, Filter filter1, Filter filter2, bool isSearching = false)
        {
            Expression bin1 = GetExpression<T>(param, filter1);
            Expression bin2 = GetExpression<T>(param, filter2);
            if (isSearching)
                return Expression.Or(bin1, bin2);
            else
                return Expression.AndAlso(bin1, bin2);
        }
    }
}
