using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
