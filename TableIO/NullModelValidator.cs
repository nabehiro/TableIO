using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableIO
{
    public class NullModelValidator : IModelValidator
    {
        public IEnumerable<ErrorDetail> Validate(object model)
        {
            return Enumerable.Empty<ErrorDetail>();
        }
    }
}
