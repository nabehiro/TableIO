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
        public ErrorDetail[] Validate(object model)
        {
            return new ErrorDetail[0];
        }
    }
}
