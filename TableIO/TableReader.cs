using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableIO
{
    public class TableReader<TModel> where TModel : new()
    {
        public bool HasHeader { get; set; }
        public int? ColumnSize { get; set; }
        public int ErrorLimit { get; set; } = 1;

        public IRowReader RowReader { get; }
        public ITypeConverterResolver TypeConverterResolver { get; }
        public IPropertyMapper PropertyMapper { get; }
        public IModelValidator ModelValidator { get; }

        private List<ErrorDetail> _errors { get; } = new List<ErrorDetail>();

        public TableReader(IRowReader rowReader, ITypeConverterResolver typeConverterResolver,
            IPropertyMapper propertyMapper, IModelValidator modelValidator)
        {
            RowReader = rowReader;
            TypeConverterResolver = typeConverterResolver;
            PropertyMapper = propertyMapper;
            ModelValidator = modelValidator;
        }

        public IEnumerable<TModel> Read()
        {
            _errors.Clear();
            var models = new List<TModel>();
            var rowIndex = 0;

            var firstRow = RowReader.Read();
            if (firstRow == null)
            {
                if (HasHeader)
                    throw new TableIOException(new[] { new ErrorDetail
                    {
                        Type = "NoTableHeader",
                        Message = "Table header is none."
                    }});
                else
                    return models;
            }

            // decide valid column size.
            // ** all row's column size must be valid column size. **
            var validColumnSize = ColumnSize ?? firstRow.Count;

            var propertyMaps = PropertyMapper.CreatePropertyMaps(typeof(TModel), HasHeader ? firstRow : null);
            var prppertyMapMaxColumnIndex = propertyMaps.Any() ? propertyMaps.Max(m => m.ColumnIndex) : -1;
            if (prppertyMapMaxColumnIndex >= validColumnSize)
                throw new TableIOException(new[] { new ErrorDetail
                    {
                        Type = "TooGreatColumnIndexMapping",
                        Message = $"Column index of property mapping is greater than or equal to valid column size({validColumnSize}).",
                        RowIndex = rowIndex
                    }});

            var row = firstRow;
            if (HasHeader)
            {
                rowIndex++;
                row = RowReader.Read();
            }

            while (row != null)
            {
                if (row.Count != validColumnSize)
                {
                    _errors.Add(new ErrorDetail
                    {
                        Type = "InvalidColumnSize",
                        Message = "Column size is invalid.",
                        RowIndex = rowIndex
                    });
                    if (_errors.Count >= ErrorLimit) throw new TableIOException(_errors);
                }
                else
                    models.Add(ConvertFromRow(row, rowIndex, propertyMaps));

                rowIndex++;
                row = RowReader.Read();
            }
            

            if (_errors.Any())
                throw new TableIOException(_errors);

            return models;
        }

        internal TModel ConvertFromRow(IList<string> row, int rowIndex, IEnumerable<PropertyMap> propertyMaps)
        {
            var model = new TModel();
            foreach(var map in propertyMaps)
            {
                var converter = TypeConverterResolver.GetTypeConverter(map.Property);
                try
                {
                    map.Property.SetValue(model, converter.ConvertFromString(row[map.ColumnIndex]));
                }
                catch(Exception ex)
                {
                    _errors.Add(new ErrorDetail
                    {
                        Type = "ConvertFailure",
                        Message = $"Field value({row[map.ColumnIndex]}) cannot be converted({ex.Message}), so property({map.Property.Name}) set is failed.",
                        RowIndex = rowIndex,
                        ColumnIndex = map.ColumnIndex
                    });
                    if (_errors.Count >= ErrorLimit) throw new TableIOException(_errors);
                }
            }

            var errors = ModelValidator.Validate(model);
            if (errors.Any())
            {
                foreach (var error in errors)
                    error.RowIndex = rowIndex;
                _errors.AddRange(errors);
                if (_errors.Count >= ErrorLimit) throw new TableIOException(_errors);
            }

            return model;
        }
    }
}
