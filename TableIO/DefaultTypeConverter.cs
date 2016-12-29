using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableIO
{
    public class DefaultTypeConverter : ITypeConverter
    {
        private TypeConverter _converter;

        public DefaultTypeConverter(TypeConverter converter)
        {
            _converter = converter;
        }

        public object ConvertFromString(string str)
        {
            return _converter.ConvertFromString(str);
        }

        public string ConvertToString(object obj)
        {
            return _converter.ConvertToString(obj);
        }
    }
}
