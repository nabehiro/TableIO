using System;
using System.Collections.Generic;
using System.Linq;
using TableIO.ModelValidators;
using TableIO.PropertyMappers;
using TableIO.RowReaders;
using TableIO.TypeConverters;

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
                    yield break;
            }

            // decide valid column size.
            // ** all row's column size must be valid column size. **
            var validColumnSize = ColumnSize ?? firstRow.Count;

            if (PropertyMapper.RequiredHeaderOnRead && !HasHeader)
                throw new TableIOException(new[] { new ErrorDetail
                    {
                        Type = "HeaderRequired",
                        Message = $"Header is required on read.",
                        RowIndex = rowIndex
                    }});

            var propertyMaps = PropertyMapper.CreatePropertyMaps(typeof(TModel), HasHeader ? firstRow.Select(f => $"{f}").ToArray() : null);
            var propertyMapMaxColumnIndex = propertyMaps.Any() ? propertyMaps.Max(m => m.ColumnIndex) : -1;
            if (propertyMapMaxColumnIndex >= validColumnSize)
                throw new TableIOException(new[] { new ErrorDetail
                    {
                        Type = "OutOfRangeColumnIndexMapping",
                        Message = $"Max column index({propertyMapMaxColumnIndex}) of property mapping is greater than or equal to valid column size({validColumnSize}).",
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
                    yield return ConvertFromRow(row, rowIndex, propertyMaps);

                rowIndex++;
                row = RowReader.Read();
            }

            if (_errors.Any())
                throw new TableIOException(_errors);
        }

        internal TModel ConvertFromRow(IList<object> row, int rowIndex, PropertyMap[] propertyMaps)
        {
            var model = new TModel();
            foreach(var map in propertyMaps)
            {
                var converter = TypeConverterResolver.GetTypeConverter(map.Property);
                try
                {
                    map.SetValue(model, converter.ConvertFromField(row[map.ColumnIndex]));
                }
                catch(Exception ex)
                {
                    _errors.Add(new ErrorDetail
                    {
                        Type = "ConvertFailed",
                        Message = $"Field value({row[map.ColumnIndex]}) cannot be converted({ex.Message}), so property({map.Property.Name}) set is failed.",
                        RowIndex = rowIndex,
                        ColumnIndex = map.ColumnIndex,
                        MemberNames = new[] { map.Property.Name }
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
