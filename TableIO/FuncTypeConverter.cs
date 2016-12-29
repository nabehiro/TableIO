using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableIO
{
    public class FuncTypeConverter : ITypeConverter
    {
        public Func<string, object> ConvertFromStringFunc { get; set; } = ConvertFromStringDefault;
        public Func<object, string> ConvertToStringFunc { get; set; } = ConvertToStringDefault;

        private static object ConvertFromStringDefault(string str)
        {
            throw new NotImplementedException();
        }

        private static string ConvertToStringDefault(object obj)
        {
            throw new NotImplementedException();
        }

        public object ConvertFromString(string str)
        {
            return ConvertFromStringFunc(str);
        }

        public string ConvertToString(object obj)
        {
            return ConvertToStringFunc(obj);
        }
    }
}
