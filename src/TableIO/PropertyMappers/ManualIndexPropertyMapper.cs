using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using TableIO.TypeConverters;

namespace TableIO.PropertyMappers
{
    public class ManualIndexPropertyMapper<T> : IPropertyMapper
    {
        private readonly Dictionary<string, Tuple<int, ITypeConverter>> _dic = new Dictionary<string, Tuple<int, ITypeConverter>>();

        public ManualIndexPropertyMapper<T> Map<TMember>(Expression<Func<T, TMember>> expression, int columnIndex)
        {
            _dic[ExpressionHelper.GetMemberName(expression)] = new Tuple<int, ITypeConverter>(columnIndex, null);
            return this;
        }

        public ManualIndexPropertyMapper<T> Map<TMember>(Expression<Func<T, TMember>> expression, int columnIndex, ITypeConverter typeConverter)
        {
            _dic[ExpressionHelper.GetMemberName(expression)] = new Tuple<int, ITypeConverter>(columnIndex, typeConverter);
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
                    ColumnIndex = _dic[p.Name].Item1,
                    Property = p,
                    TypeConverter = _dic[p.Name].Item2
                })
                .ToArray();
        }
    }
}
