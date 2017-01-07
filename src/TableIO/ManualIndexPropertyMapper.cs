using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TableIO
{
    public class ManualIndexPropertyMapper<T> : IPropertyMapper
    {
        private readonly Dictionary<string, int> _dic = new Dictionary<string, int>();

        public ManualIndexPropertyMapper<T> Map<TMember>(Expression<Func<T, TMember>> expression, int columnIndex)
        {
            _dic[ExpressionHelper.GetMemberName(expression)] = columnIndex;
            return this;
        }

        public PropertyMap[] CreatePropertyMaps(Type type, IList<string> header)
        {
            var names = _dic.Keys.ToArray();

            return TypeDescriptor.GetProperties(type)
                .OfType<PropertyDescriptor>()
                .Where(p => names.Contains(p.Name))
                .Select(p => new PropertyMap
                {
                    ColumnIndex = _dic[p.Name],
                    Property = p
                })
                .ToArray();
        }
    }
}
