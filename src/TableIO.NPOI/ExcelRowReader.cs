using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using TableIO.RowReaders;

namespace TableIO.NPOI
{
    public class ExcelRowReader : IRowReader
    {
        private readonly ISheet _worksheet;
        private readonly int _startColumnIndex;
        private readonly int _columnSize;

        private int _currentRowIndex;

        public ExcelRowReader(ISheet worksheet, int startRowNumber, int startColumnNumber, int columnSize)
        {
            _worksheet = worksheet;
            _currentRowIndex = startRowNumber - 1;
            _startColumnIndex = startColumnNumber - 1;
            _columnSize = columnSize;
        }

        public IList<object> Read()
        {
            var xlsRow = _worksheet.GetRow(_currentRowIndex);
            if (xlsRow == null)
                return null;

            var fields = Enumerable.Range(_startColumnIndex, _columnSize)
                .Select(ci => GetCellValue(xlsRow.GetCell(ci)))
                .ToArray();

            if (fields.All(f => f == null))
                return null;

            _currentRowIndex++;
            return fields;
        }

        public static object GetCellValue(ICell cell)
        {
            return cell != null ? GetCellValue(cell, cell.CellType) : null;
        }

        private static object GetCellValue(ICell cell, CellType cellType)
        {
            switch(cellType)
            {
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Blank:
                    return null;
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                        return cell.DateCellValue;
                    else
                        return cell.NumericCellValue;
                case CellType.Formula:
                    return GetCellValue(cell, cell.CachedFormulaResultType);
                default:
                    throw new InvalidOperationException($"Celltype is unexpected type.(${cellType})");
            }
        }
    }
}
