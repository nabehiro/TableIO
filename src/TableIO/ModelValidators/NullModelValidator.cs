namespace TableIO.ModelValidators
{
    public class NullModelValidator : IModelValidator
    {
        public ErrorDetail[] Validate(object model)
        {
            return new ErrorDetail[0];
        }
    }
}
