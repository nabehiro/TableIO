using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace TableIO.TypeConverters
{
    public class DefaultTypeConverterResolver<T> : ITypeConverterResolver
    {
        private readonly Dictionary<string, ITypeConverter> _dic = new Dictionary<string, ITypeConverter>();

        public ITypeConverter GetTypeConverter(PropertyInfo property)
        {
            if (!_dic.ContainsKey(property.Name))
            {
                var converter = TypeDescriptor.GetConverter(property.PropertyType);
                _dic[property.Name] = new DefaultTypeConverter(converter);
            }
            return _dic[property.Name];
        }

        public DefaultTypeConverterResolver<T> SetTypeConverter<TMember>(Expression<Func<T, TMember>> expression, ITypeConverter typeConverter)
        {
            var name = ExpressionHelper.GetMemberName(expression);
            _dic[name] = typeConverter;
            return this;
        }
    }
}
