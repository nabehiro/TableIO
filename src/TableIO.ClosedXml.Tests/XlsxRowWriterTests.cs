using ClosedXML.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TableIO.ClosedXml.Tests
{
    [TestClass()]
    public class XlsxRowWriterTests
    {
        class Model
        {

        }

        [TestMethod()]
        public void WriteTest()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("sample");

                var rowWriter = new XlsxRowWriter(worksheet, 2, 4);
                rowWriter.Write(new object[] { 1, 2, 3, 4 });
                rowWriter.Write(new object[] { "1", "2", "3", "4" });
                rowWriter.Write(new object[] { new DateTime(2016,1,1), new DateTime(2016,1,1,1,1,1) });
                rowWriter.Write(new object[] { true, "", false });
                rowWriter.Write(new object[] { new Model() });

                workbook.SaveAs("files/WriterTest.xlsx");
            }
        }
    }
}