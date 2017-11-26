using System.ComponentModel;
using TableIO.TypeConverters;

namespace TableIO.PropertyMappers
{
    public class PropertyMap
    {
        public int ColumnIndex { get; set; }
        public PropertyDescriptor Property { get; set; }
        public ITypeConverter TypeConverter { get; set; }
    }
}
