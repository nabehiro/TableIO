using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace TableIO.PropertyMappers
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

            return type.GetProperties()
                .Where(p => names.Contains(p.Name))
                .Select(p => new PropertyMap
                {
                    ColumnIndex = _dic[p.Name],
                    Property = p,
                    GetValue = (obj) => p.GetValue(obj),
                    SetValue = (obj, val) => p.SetValue(obj, val)
                })
                .ToArray();
        }
    }
}
