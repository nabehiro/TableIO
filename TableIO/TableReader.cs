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
        public int? ValidColumnSize { get; set; }
        public int ErrorLimit { get; set; } = 1;

        public IRowReader RowReader { get; set; }
        public ITypeConverterResolver TypeConverterResolver { get; set; }
        public IPropertyMapper PropertyMapper { get; set; }
        public IModelValidator ModelValidator { get; set; }

        public List<ErrorDetail> Errors { get; } = new List<ErrorDetail>();

        public IEnumerable<TModel> Read()
        {
            Errors.Clear();
            var models = new List<TModel>();
            var rowIndex = 0;


            var firstRow = RowReader.Read();

            if (firstRow != null)
            {
                if (!ValidColumnSize.HasValue)
                    ValidColumnSize = firstRow.Length;
                else if (ValidColumnSize != firstRow.Length)
                {
                    Errors.Add(new ErrorDetail
                    {
                        Type = "InvalidColumnSize",
                        Message = "Column size is invalid.",
                        RowIndex = rowIndex
                    });
                    if (Errors.Count >= ErrorLimit) throw new TableIOException(Errors);
                }
            }

            if (HasHeader)
            {
                if (firstRow == null)
                {
                    Errors.Add(new ErrorDetail
                    {
                        Type = "NoTableHeader",
                        Message = "Table header is none."
                    });
                    throw new TableIOException(Errors);
                }

                PropertyMapper.SetTableHeader(firstRow);
            }

            var propertyMaps = PropertyMapper.CreatePropertyMaps();

            if (!HasHeader)
            {
                if (firstRow != null)
                    models.Add(ConvertFromRow(firstRow, rowIndex, propertyMaps));
                else
                    return models;
            }

            while (true)
            {
                rowIndex++;
                var row = RowReader.Read();
                if (row == null)
                    break;
                if (row.Length != ValidColumnSize)
                {
                    Errors.Add(new ErrorDetail
                    {
                        Type = "InvalidColumnSize",
                        Message = "Column size is invalid.",
                        RowIndex = rowIndex
                    });
                    if (Errors.Count >= ErrorLimit) throw new TableIOException(Errors);
                }

                models.Add(ConvertFromRow(row, rowIndex, propertyMaps));
            }

            return models;
        }

        internal TModel ConvertFromRow(string[] row, int rowIndex, IEnumerable<PropertyMap> propertyMaps)
        {
            var model = new TModel();
            foreach(var map in propertyMaps)
            {
                if (map.ColumnIndex >= row.Length)
                {
                    Errors.Add(new ErrorDetail
                    {
                        Type = "OverColumnIndex",
                        Message = "Column index of property mapping is over column size of reading row.",
                        RowIndex = rowIndex,
                        ColumnIndex = map.ColumnIndex
                    });
                    if (Errors.Count >= ErrorLimit) throw new TableIOException(Errors);
                }

                var converter = TypeConverterResolver.GetTypeConverter(map.Property);
                try
                {
                    map.Property.SetValue(model, converter.ConvertFromString(row[map.ColumnIndex]));
                }
                catch(Exception ex)
                {
                    Errors.Add(new ErrorDetail
                    {
                        Type = "IllegalTypeConvert",
                        Message = $"Type conver is illegal({ex.Message}).",
                        RowIndex = rowIndex,
                        ColumnIndex = map.ColumnIndex
                    });
                    if (Errors.Count >= ErrorLimit) throw new TableIOException(Errors);
                }
            }

            var errors = ModelValidator.Validate(model);
            if (errors.Any())
            {
                foreach (var error in errors)
                    error.RowIndex = rowIndex;
                Errors.AddRange(errors);
                if (Errors.Count >= ErrorLimit) throw new TableIOException(Errors);
            }

            return model;
        }
    }
}
