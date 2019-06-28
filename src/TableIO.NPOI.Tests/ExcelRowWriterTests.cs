using Microsoft.VisualStudio.TestTools.UnitTesting;
using TableIO.NPOI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;

namespace TableIO.NPOI.Tests
{
    [TestClass()]
    public class ExcelRowWriterTests
    {
        class Model
        {

        }

        [TestMethod()]
        public void WriteTest()
        {
            IWorkbook workbook = new HSSFWorkbook();
            var worksheet = workbook.CreateSheet("sample");

            var rowWriter = new ExcelRowWriter(worksheet, 2, 4);
            rowWriter.Write(new object[] { 1, 2, 3, 4 });
            rowWriter.Write(new object[] { "1", "2", "3", "4" });
            rowWriter.Write(new object[] { new DateTime(2016, 1, 1), new DateTime(2016, 1, 1, 1, 1, 1) });
            rowWriter.Write(new object[] { true, "", false });
            rowWriter.Write(new object[] { new Model() });

            using (var stream = new FileStream("files/WriterTest.xls", FileMode.Create, FileAccess.Write))
                workbook.Write(stream);
        }
    }
}