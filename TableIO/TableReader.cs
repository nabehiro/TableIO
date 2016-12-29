using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableIO
{
    public class TableReader
    {
        public bool HasHeader { get; set; }
        public int ValidColumnSize { get; set; } = -1;

        public IRowReader RowReader { get; set; }
        public ITypeConverterResolver TypeConverterResolver { get; set; }
        public IPropertyMapper PropertyMapper { get; set; }

        private int _rowNumber = 0;
        private IEnumerable<PropertyMap> _propertyMaps = null;

        public IEnumerable<T> Read<T>()
            where T : new()
        {
            var models = new List<T>();

            _rowNumber++;
            var firstRow = RowReader.Read();

            if (firstRow != null)
            {
                if (ValidColumnSize == -1)
                    ValidColumnSize = firstRow.Length;
                else if (ValidColumnSize != firstRow.Length)
                    throw new TableIOException($"{_rowNumber}: column size is invalid.");
            }

            if (HasHeader)
            {
                if (firstRow == null)
                    throw new TableIOException("header is null.");
                PropertyMapper.SetTableHeader(firstRow);
            }

            _propertyMaps = PropertyMapper.CreatePropertyMaps();

            if (!HasHeader)
            {
                if (firstRow != null)
                    models.Add(ConvertFromRow<T>(firstRow));
                else
                    return models;
            }

            while (true)
            {
                _rowNumber++;
                var row = RowReader.Read();
                if (row == null)
                    break;
                if (row.Length != ValidColumnSize)
                    throw new TableIOException($"{_rowNumber}: column size is invalid.");

                models.Add(ConvertFromRow<T>(row));
            }

            return models;
        }

        public T ConvertFromRow<T>(string[] row)
            where T : new()
        {
            var model = new T();
            foreach(var map in _propertyMaps)
            {
                if (map.Index >= row.Length)
                    throw new TableIOException($"{_rowNumber}: {map.Property.Name}(index:{map.Index}) is over row size({row.Length}).");

                var converter = TypeConverterResolver.GetTypeConverter(map.Property);
                try
                {
                    map.Property.SetValue(model, converter.ConvertFromString(row[map.Index]));
                }
                catch(Exception ex)
                {
                    throw new TableIOException($"{_rowNumber}: {map.Property.Name}(index:{map.Index}) cannot be set {row[map.Index]}.", ex);
                }
            }
            return model;
        }
    }
}
