using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableIO
{
    public class NullModelValidator : IModelValidator
    {
        public IEnumerable<string> Validate(object model)
        {
            return Enumerable.Empty<string>();
        }
    }
}
