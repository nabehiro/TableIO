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

        public object ConvertFromField(object fieldValue)
        {
            return _converter.ConvertFromString(fieldValue == null ? "" : fieldValue.ToString());
        }

        public object ConvertToField(object propertyValue)
        {
            return propertyValue;
        }
    }
}
