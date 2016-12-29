using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableIO
{
    public interface ITypeConverter
    {
        string ConvertToString(object obj);
        object ConvertFromString(string str);
    }
}
