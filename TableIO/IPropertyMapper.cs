using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableIO
{
    public interface IPropertyMapper
    {
        IEnumerable<PropertyMap> CreatePropertyMaps();
        void SetTableHeader(string[] header);
    }
}
