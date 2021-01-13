using System.Collections.Generic;
using System.Linq;
using TableIO.PropertyMappers;
using TableIO.RowWriters;
using TableIO.TypeConverters;

namespace TableIO
{
    public class TableWriter<TModel>
    {
        public int? ColumnSize { get; set; }

        public IRowWriter RowWriter { get; }
        public ITypeConverterResolver TypeConverterResolver { get; }
        public IPropertyMapper PropertyMapper { get; }

        public TableWriter(IRowWriter rowWriter, ITypeConverterResolver typeConverterResolver, IPropertyMapper propertyMapper)
        {
            RowWriter = rowWriter;
            TypeConverterResolver = typeConverterResolver;
            PropertyMapper = propertyMapper;
        }

        public void Write(IEnumerable<TModel> models, IList<string> header = null)
        {
            if (PropertyMapper.RequiredHeaderOnWrite && header == null)
                throw new TableIOException(new[] { new ErrorDetail
                    {
                        Type = "HeaderRequired",
                        Message = $"Header is required on write.",
                    }});

            var propertyMaps = PropertyMapper.CreatePropertyMaps(typeof(TModel), header);

            // decide valid column size.
            var validColumnSize = propertyMaps.Any() ? propertyMaps.Max(m => m.ColumnIndex) + 1 : 0;
            if (ColumnSize.HasValue)
            {
                if (ColumnSize < validColumnSize)
                    throw new TableIOException(new[] { new ErrorDetail
                    {
                        Type = "InvalidColumnSize",
                        Message = "Column size is invalid.",
                    }});

                validColumnSize = ColumnSize.Value;
            }
            else if (header != null)
            {
                if (header.Count < validColumnSize)
                    throw new TableIOException(new[] { new ErrorDetail
                    {
                        Type = "InvalidTableHeader",
                        Message = "Table header is invalid.",
                    }});

                validColumnSize = header.Count;
                RowWriter.Write(header.Cast<object>().ToArray());
            }

            foreach (var model in models)
                RowWriter.Write(ConvertToRow(model, propertyMaps, validColumnSize));
        }

        internal IList<object> ConvertToRow(TModel model, PropertyMap[] propertyMaps, int columnSize)
        {
            var row = new object[columnSize];

            foreach (var map in propertyMaps)
            {
                var converter = TypeConverterResolver.GetTypeConverter(map.Property);
                row[map.ColumnIndex] = converter.ConvertToField(map.GetValue(model));
            }

            return row;
        }
    }
}
