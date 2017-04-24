using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableIO.RowWriters;

namespace TableIO.NPOI
{
    public class ExcelRowWriter : IRowWriter
    {
        private readonly ISheet _worksheet;
        private readonly int _startColumnIndex;

        private int _currentRowIndex;

        public ExcelRowWriter(ISheet worksheet, int startRowNumber, int startColumnNumber)
        {
            _worksheet = worksheet;
            _currentRowIndex = startRowNumber - 1;
            _startColumnIndex = startColumnNumber - 1;
        }

        public void Write(IList<object> row)
        {
            var xlsRow = _worksheet.GetRow(_currentRowIndex) ?? _worksheet.CreateRow(_currentRowIndex);
            for (int i = 0; i < row.Count; i++)
            {
                var xlsCell = xlsRow.GetCell(_startColumnIndex + i) ?? xlsRow.CreateCell(_startColumnIndex + i);
                SetCellValue(xlsCell, row[i]);
            }

            _currentRowIndex++;
        }

        private static readonly Type[] _boolTypes = new[] { typeof(bool), typeof(bool?) };
        private static readonly Type[] _doubleTypes = new[]
        {
            typeof(sbyte), typeof(sbyte?),
            typeof(short), typeof(short?),
            typeof(int), typeof(int?),
            typeof(long), typeof(long?),
            typeof(byte), typeof(byte?),
            typeof(ushort), typeof(ushort?),
            typeof(uint), typeof(uint?),
            typeof(ulong), typeof(ulong?),
            typeof(float), typeof(float?),
            typeof(double), typeof(double?),
            typeof(decimal), typeof(decimal?)
        };


        private void SetCellValue(ICell cell, object value)
        {
            if (value == null)
                return;
            var type = value.GetType();

            if (type == typeof(string))
                cell.SetCellValue((string)value);
            else if (_boolTypes.Contains(type))
                cell.SetCellValue((bool)value);
            else if (_doubleTypes.Contains(type))
                cell.SetCellValue(Convert.ToDouble(value));
            else
                cell.SetCellValue(value.ToString());
        }
    }
}
