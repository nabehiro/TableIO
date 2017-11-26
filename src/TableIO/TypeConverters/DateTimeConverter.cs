using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableIO.TypeConverters
{
    public class DateTimeConverter : TypeConverterBase<DateTime, string>
    {
        private readonly string _format;

        public DateTimeConverter(string format)
        {
            _format = format;
        }

        protected override string ConvertToFieldValue(DateTime propertyValue)
        {
            return propertyValue.ToString(_format);
        }

        protected override DateTime ConvertFromPropertyValue(string fieldValue)
        {
            return DateTime.ParseExact(fieldValue, _format, System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
    }
}
