using System;

namespace TableIO.TypeConverters
{
    public class FuncTypeConverter : ITypeConverter
    {
        public Func<object, object> ConvertFromFieldFunc { get; set; } = ConvertFromFieldDefault;
        public Func<object, object> ConvertToFieldFunc { get; set; } = ConvertToFieldDefault;

        private static object ConvertFromFieldDefault(object fieldValue)
        {
            throw new NotImplementedException();
        }

        private static object ConvertToFieldDefault(object propertyValue)
        {
            throw new NotImplementedException();
        }

        public object ConvertFromField(object fieldValue)
        {
            return ConvertFromFieldFunc(fieldValue);
        }

        public object ConvertToField(object propertyValue)
        {
            return ConvertToFieldFunc(propertyValue);
        }
    }
}
