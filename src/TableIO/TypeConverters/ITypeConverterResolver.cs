using System.Reflection;

namespace TableIO.TypeConverters
{
    public interface ITypeConverterResolver
    {
        ITypeConverter GetTypeConverter(PropertyInfo property);
    }
}
