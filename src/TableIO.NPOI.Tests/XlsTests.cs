using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;
using System.Linq;

namespace TableIO.NPOI.Tests
{
    
    [TestClass]
    public class XlsTests
    {
        [TestMethod]
        public void Read()
        {
            using (var stream = new FileStream("files\\Read.xls", FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new HSSFWorkbook(stream);
                var worksheet = workbook.GetSheetAt(0);

                var v1 = worksheet.GetRow(0).GetCell(1);  // 1
                Assert.AreEqual(CellType.Numeric, v1.CellType);
                Assert.AreEqual(1D, v1.NumericCellValue);
                var v2 = worksheet.GetRow(1).GetCell(1);  // 1.23
                Assert.AreEqual(1.23, v2.NumericCellValue);
                var v3 = worksheet.GetRow(2).GetCell(1);  // 3000000000
                Assert.AreEqual(3000000000D, v3.NumericCellValue);

                var v4 = worksheet.GetRow(3).GetCell(1);  // "abc"
                Assert.AreEqual(CellType.String, v4.CellType);
                Assert.AreEqual("abc", v4.StringCellValue);
                var v5 = worksheet.GetRow(4).GetCell(1);  // "0123"
                Assert.AreEqual("0123", v5.StringCellValue);

                var v6 = worksheet.GetRow(5).GetCell(1);  // 2016/01/01
                var isDateCell = DateUtil.IsCellDateFormatted(v6);
                Assert.IsTrue(isDateCell);
                Assert.AreEqual(new DateTime(2017, 1, 1), v6.DateCellValue);

                var v7 = worksheet.GetRow(6).GetCell(1);  // true
                Assert.AreEqual(CellType.Boolean, v7.CellType);
                Assert.AreEqual(true, v7.BooleanCellValue);
                var v8 = worksheet.GetRow(7).GetCell(1);  // false
                Assert.AreEqual(false, v8.BooleanCellValue);

                Assert.AreEqual(null, worksheet.GetRow(7).GetCell(10)); // null(empty cell)

                var v9 = worksheet.GetRow(8).GetCell(1);  // 1 + 1
                Assert.AreEqual(CellType.Formula, v9.CellType);
                Assert.AreEqual(CellType.Numeric, v9.CachedFormulaResultType);
                Assert.AreEqual(2D, v9.NumericCellValue);

                var v10 = worksheet.GetRow(9).GetCell(1);  // "a" + "b"
                Assert.AreEqual(CellType.Formula, v10.CellType);
                Assert.AreEqual(CellType.String, v10.CachedFormulaResultType);
                Assert.AreEqual("ab", v10.StringCellValue);

                Assert.AreEqual(null, worksheet.GetRow(100)); // null(empty row)
            }

        }

        [TestMethod]
        public void Write()
        {
            IWorkbook workbook = new HSSFWorkbook();
            var worksheet = workbook.CreateSheet("hoge");

            worksheet.CreateRow(0).CreateCell(0).SetCellValue(1);
            Assert.AreEqual(1D, worksheet.GetRow(0).GetCell(0).NumericCellValue);

            worksheet.CreateRow(1).CreateCell(0).SetCellValue(1.23);
            Assert.AreEqual(1.23D, worksheet.GetRow(1).GetCell(0).NumericCellValue);

            worksheet.CreateRow(2).CreateCell(0).SetCellValue("abc");
            Assert.AreEqual("abc", worksheet.GetRow(2).GetCell(0).StringCellValue);

            worksheet.CreateRow(3).CreateCell(0).SetCellValue(new DateTime(2017, 1, 1, 1, 1, 1));
            Assert.AreEqual(new DateTime(2017, 1, 1, 1, 1, 1), worksheet.GetRow(3).GetCell(0).DateCellValue);

            // change CellStyle of Date Value cell.
            var dateFormat = workbook.CreateDataFormat().GetFormat("yyyy/MM/dd HH:mm:ss");
            var dateStyle = workbook.CreateCellStyle();
            dateStyle.DataFormat = dateFormat;
            worksheet.GetRow(3).GetCell(0).CellStyle = dateStyle;

            worksheet.CreateRow(4).CreateCell(0).SetCellValue(false);
            Assert.AreEqual(false, worksheet.GetRow(4).GetCell(0).BooleanCellValue);

            using (var stream = new FileStream("files\\Write.xls", FileMode.Create, FileAccess.Write))
                workbook.Write(stream);
        }

        [TestMethod]
        public void WriteTemplate()
        {
            IWorkbook workbook = null;
            using (var stream = new FileStream("files\\Template.xls", FileMode.Open, FileAccess.Read))
                workbook = new HSSFWorkbook(stream);

            var worksheet = workbook.GetSheetAt(0);
            var row0 = worksheet.GetRow(0);
            Assert.IsNotNull(row0);
            var row1 = worksheet.GetRow(1);
            Assert.IsNull(row1);

            row1 = worksheet.CreateRow(1);
            row1.CreateCell(0).SetCellValue(new DateTime(2017, 1, 1, 1, 1, 1));
            row1.CreateCell(1).SetCellValue(new DateTime(2017, 1, 1, 1, 1, 1));
            SetCellValue(row1.CreateCell(2), new object());
            SetCellValue(row1.CreateCell(3), 100.00m);

            using (var stream = new FileStream("files\\WriteTemplate.xls", FileMode.Create, FileAccess.Write))
                workbook.Write(stream);
        }

        private static Type[] _boolTypes = new[] { typeof(bool), typeof(bool?) };
        private static Type[] _doubleTypes = new[]
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
