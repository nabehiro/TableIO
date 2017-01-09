namespace TableIO.ModelValidators
{
    public interface IModelValidator
    {
        ErrorDetail[] Validate(object model);
    }
}
