using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableIO
{
    public class AutoIndexPropertyMapper : IPropertyMapper
    {
        public IEnumerable<PropertyMap> CreatePropertyMaps(Type type, IList<string> header)
        {
            return TypeDescriptor.GetProperties(type)
                .OfType<PropertyDescriptor>()
                .Select((p, i) => new PropertyMap
                {
                    ColumnIndex = i, Property = p
                });
        }
    }
}
