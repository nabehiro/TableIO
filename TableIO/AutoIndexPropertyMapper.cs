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
        private Type _type;

        public AutoIndexPropertyMapper(Type type)
        {
            _type = type;
        }

        public IEnumerable<PropertyMap> CreatePropertyMaps()
        {
            return TypeDescriptor.GetProperties(_type)
                .OfType<PropertyDescriptor>()
                .Select((p, i) => new PropertyMap
                {
                    ColumnIndex = i, Property = p
                });
        }

        public void SetTableHeader(IList<string> header)
        {
        }
    }
}
