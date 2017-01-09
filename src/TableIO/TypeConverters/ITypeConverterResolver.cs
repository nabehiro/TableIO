using System.ComponentModel;

namespace TableIO.TypeConverters
{
    public interface ITypeConverterResolver
    {
        ITypeConverter GetTypeConverter(PropertyDescriptor property);
    }
}
