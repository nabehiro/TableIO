namespace TableIO.TypeConverters
{
    public interface ITypeConverter
    {
        object ConvertToField(object propertyValue);
        object ConvertFromField(object fieldValue);
    }
}
