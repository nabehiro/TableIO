using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TableIO.PropertyMappers
{
    public class AutoIndexPropertyMapper : IPropertyMapper
    {
        public PropertyMap[] CreatePropertyMaps(Type type, IList<string> header)
        {
            return type.GetProperties()
                .Select((p, i) => new PropertyMap
                {
                    ColumnIndex = i, Property = p
                })
                .ToArray();
        }
    }
}
