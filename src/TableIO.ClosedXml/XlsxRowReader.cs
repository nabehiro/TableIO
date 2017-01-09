using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableIO.RowReaders;

namespace TableIO.ClosedXml
{
    public class XlsxRowReader : IRowReader
    {
        private readonly IXLWorksheet _worksheet;
        private readonly int _startColumnNumber;
        private readonly int _lastColumnNumber;

        private int _currentRowNumber;

        public XlsxRowReader(IXLWorksheet worksheet, int startRowNumber, int startColumnNumber, int columnSize)
        {
            _worksheet = worksheet;
            _currentRowNumber = startRowNumber;
            _startColumnNumber = startColumnNumber;
            _lastColumnNumber = startColumnNumber + columnSize - 1;
        }

        public IList<object> Read()
        {
            var row = _worksheet.Row(_currentRowNumber);
            var fields = row
                .Cells(_startColumnNumber, _lastColumnNumber)
                .Select(c => c.Value)
                .ToArray();

            if (fields.All(f => (f as string) == ""))
                return null;

            _currentRowNumber++;
            return fields;
        }
    }
}
