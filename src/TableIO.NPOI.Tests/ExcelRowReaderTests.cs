using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Linq;
using TableIO.NPOI;

namespace TableIO.NPOI.Tests
{
    [TestClass()]
    public class ExcelRowReaderTests
    {
        [TestMethod()]
        public void ReadTest()
        {
            // header + 5 row
            using (var stream = new FileStream("files/Valid.xlsx", FileMode.Open, FileAccess.Read))
            {
                var workbook = new XSSFWorkbook(stream);
                var worksheet = workbook.GetSheetAt(0);

                var rowReader = new ExcelRowReader(worksheet, 1, 1, 4);

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
}