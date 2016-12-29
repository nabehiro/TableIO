using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableIO
{
    public interface ITypeConverterResolver
    {
        ITypeConverter GetTypeConverter(PropertyDescriptor property);
    }
}
