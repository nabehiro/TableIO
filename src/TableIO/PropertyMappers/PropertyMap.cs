using System;
using System.Reflection;

namespace TableIO.PropertyMappers
{
    public class PropertyMap
    {
        public int ColumnIndex { get; set; }
        public PropertyInfo Property { get; set; }
        public Func<object, object> GetValue { get; set; }
        public Action<object, object> SetValue { get; set; }
    }
}
