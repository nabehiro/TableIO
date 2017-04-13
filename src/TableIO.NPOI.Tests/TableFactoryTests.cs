using Microsoft.VisualStudio.TestTools.UnitTesting;
using TableIO.NPOI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;

namespace TableIO.NPOI.Tests
{
    [TestClass()]
    public class TableFactoryTests
    {
        class Model
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Remarks { get; set; }
        }

        [TestMethod]
        public void CreateExcelWriter()
        {
            IWorkbook workbook = new HSSFWorkbook();

            var worksheet = workbook.CreateSheet("sample");

            var models = new[]
            {
                    new Model { Id = 10, Name = "NAME_10", Price = 100, Remarks = "REMARKS_10"
                    },
                    new Model { Id = 20, Name = "NAME_20", Price = 200, Remarks = null
                    },
                    new Model { Id = 30, Name = "NAME_30", Price = 300, Remarks = "REMARKS_30"
                    }
                };

            var tableWriter = new TableFactory().CreateExcelWriter<Model>(worksheet, 1, 1);
            tableWriter.Write(models, new[] { "ID", "NAME", "PRICE", "REMARKS" });

            Assert.AreEqual("ID", worksheet.GetRow(0).GetCell(0).StringCellValue);
            Assert.AreEqual("REMARKS", worksheet.GetRow(0).GetCell(3).StringCellValue);

            Assert.AreEqual(10D, worksheet.GetRow(1).GetCell(0).NumericCellValue);
            Assert.AreEqual("REMARKS_10", worksheet.GetRow(1).GetCell(3).StringCellValue);

            Assert.AreEqual("", worksheet.GetRow(2).GetCell(3).StringCellValue);

            Assert.AreEqual(300D, worksheet.GetRow(3).GetCell(2).NumericCellValue);
        }
    }
}