using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableIO
{
    public class DefaultModelValidator : IModelValidator
    {
        public IEnumerable<ErrorDetail> Validate(object model)
        {
            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, context, results, true);

            return results.Select(r => new ErrorDetail
            {
                Type = "ModelValidation",
                Message = r.ErrorMessage,
                MemberNames = r.MemberNames
            })
            .ToArray();
        }
    }
}
