using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace TableIO.TypeConverters
{
    public class DefaultTypeConverterResolver<T> : ITypeConverterResolver
    {
        private readonly Dictionary<PropertyInfo, ITypeConverter> _dic = new Dictionary<PropertyInfo, ITypeConverter>();

        public ITypeConverter GetTypeConverter(PropertyInfo property)
        {
            if (!_dic.ContainsKey(property))
            {
                var converter = TypeDescriptor.GetConverter(property.PropertyType);
                _dic[property] = new DefaultTypeConverter(converter);
            }
            return _dic[property];
        }

        public DefaultTypeConverterResolver<T> SetTypeConverter(PropertyInfo property, ITypeConverter typeConverter)
        {
            _dic[property] = typeConverter;
            return this;
        }
    }
}
