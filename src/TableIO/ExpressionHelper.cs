using System;
using System.Linq.Expressions;

namespace TableIO
{
    internal static class ExpressionHelper
    {
        public static string GetMemberName<T, TMember>(Expression<Func<T, TMember>> expression)
        {
            return ((MemberExpression)expression.Body).Member.Name;
        }
    }
}
