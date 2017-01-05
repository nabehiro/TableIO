using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableIO.ClosedXml
{
    public class XlsxRowWriter : IRowWriter
    {
        private readonly IXLWorksheet _worksheet;
        private readonly int _startColumnNumber;

        private int _currentRowNumber;

        public XlsxRowWriter(IXLWorksheet worksheet, int startRowNumber, int startColumnNumber)
        {
            _worksheet = worksheet;
            _currentRowNumber = startRowNumber;
            _startColumnNumber = startColumnNumber;
        }

        public void Write(IList<object> row)
        {
            for (int i = 0; i < row.Count; i++)
                _worksheet.Cell(_currentRowNumber, _startColumnNumber + i).SetValue(row[i]);

            _currentRowNumber++;
        }
    }
}
