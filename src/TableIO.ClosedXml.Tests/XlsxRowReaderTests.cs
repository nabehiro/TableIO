using ClosedXML.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace TableIO.ClosedXml.Tests
{
    [TestClass()]
    public class XlsxRowReaderTests
    {
        [TestMethod()]
        public void ReadTest()
        {
            // header + 5 row
            var workbook = new XLWorkbook("files\\Valid.xlsx");
            var worksheet = workbook.Worksheet(1);

            var rowReader = new XlsxRowReader(worksheet, 1, 1, 4);

            var row = rowReader.Read();
            CollectionAssert.AreEqual(new[] { "ID", "NAME", "PRICE", "REMARKS" }, row.ToArray());

            row = rowReader.Read();
            Assert.AreEqual(4, row.Count);
            row = rowReader.Read();
            Assert.AreEqual(4, row.Count);
            row = rowReader.Read();
            Assert.AreEqual(4, row.Count);
            row = rowReader.Read();
            Assert.AreEqual(4, row.Count);
            row = rowReader.Read();
            Assert.AreEqual(4, row.Count);
            row = rowReader.Read();
            Assert.IsNull(row);
        }
    }
}