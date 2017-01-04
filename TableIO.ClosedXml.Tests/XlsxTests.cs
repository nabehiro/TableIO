using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClosedXML.Excel;

namespace TableIO.ClosedXml.Tests
{
    [TestClass]
    public class XlsxTests
    {
        [TestMethod]
        public void Read()
        {
            using (var workbook = new XLWorkbook("files\\Read.xlsx"))
            {
                var worksheet = workbook.Worksheet(1);

                // value of CellType(NUMBER) turns out double
                var v1 = worksheet.Cell(1, 2);  // 1
                Assert.AreEqual(1D, v1.Value);
                var v2 = worksheet.Cell(2, 2);  // 1.23
                Assert.AreEqual(1.23, v2.Value);
                var v3 = worksheet.Cell(3, 2);  // 3000000000
                Assert.AreEqual(3000000000D, v3.Value);

                var v4 = worksheet.Cell(4, 2);  // "abc"
                Assert.AreEqual("abc", v4.Value);
                var v5 = worksheet.Cell(5, 2);  // "0123"
                Assert.AreEqual("0123", v5.Value);

                var v6 = worksheet.Cell(6, 2);  // 2016/01/01
                Assert.AreEqual(new DateTime(2017,1,1), v6.Value);

                var v7 = worksheet.Cell(7, 2);  // true
                Assert.AreEqual(true, v7.Value);
                var v8 = worksheet.Cell(8, 2);  // false
                Assert.AreEqual(false, v8.Value);

                var v9 = worksheet.Cell(9, 2);  // null(empty cell)
                Assert.AreEqual("", v9.Value);
            }
        }

        [TestMethod]
        public void Write()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("sample");

                worksheet.Cell(1, 1).SetValue((int)1);
                Assert.AreEqual(1D, worksheet.Cell(1, 1).Value);

                worksheet.Cell(2, 1).SetValue((double)1.23);
                Assert.AreEqual(1.23D, worksheet.Cell(2, 1).Value);

                worksheet.Cell(3, 1).SetValue((decimal)1);
                Assert.AreEqual(1D, worksheet.Cell(3, 1).Value);

                worksheet.Cell(4, 1).SetValue((long)3000000000);
                Assert.AreEqual(3000000000D, worksheet.Cell(4, 1).Value);

                worksheet.Cell(5, 1).SetValue("abc");
                Assert.AreEqual("abc", worksheet.Cell(5, 1).Value);

                worksheet.Cell(6, 1).SetValue("0123");
                Assert.AreEqual("0123", worksheet.Cell(6, 1).Value);

                worksheet.Cell(7, 1).SetValue("bbb").SetDataType(XLCellValues.Text);
                Assert.AreEqual("bbb", worksheet.Cell(7, 1).Value);

                worksheet.Cell(8, 1).SetValue(new DateTime(2017, 1, 1, 1, 1, 1));
                Assert.AreEqual(new DateTime(2017, 1, 1, 1, 1, 1), worksheet.Cell(8, 1).Value);

                worksheet.Cell(9, 1).SetValue(true);
                Assert.AreEqual(true, worksheet.Cell(9, 1).Value);

                worksheet.Cell(10, 1).SetValue(false);
                Assert.AreEqual(false, worksheet.Cell(10, 1).Value);

                worksheet.Cell(11, 1).SetValue((object)null);
                Assert.AreEqual("", worksheet.Cell(11, 1).Value);


                workbook.SaveAs("files\\Write.xlsx");
            }
        }
    }
}
