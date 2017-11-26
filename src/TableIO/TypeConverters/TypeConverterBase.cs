using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableIO.TypeConverters
{
    public abstract class TypeConverterBase<TPropertyValue, TFieldValue> : ITypeConverter
    {
        protected abstract TFieldValue ConvertToFieldValue(TPropertyValue propertyValue);

        protected abstract TPropertyValue ConvertFromPropertyValue(TFieldValue fieldValue);

        public object ConvertToField(object propertyValue)
        {
            return ConvertToFieldValue((TPropertyValue)propertyValue);
        }

        public object ConvertFromField(object fieldValue)
        {
            return ConvertFromPropertyValue((TFieldValue)fieldValue);
        }
    }
}
