using System;
using System.Collections.Generic;

namespace TableIO.PropertyMappers
{
    public interface IPropertyMapper
    {
        PropertyMap[] CreatePropertyMaps(Type type, IList<string> header);

        bool RequiredHeaderOnRead { get; }
        bool RequiredHeaderOnWrite { get; }
    }
}
