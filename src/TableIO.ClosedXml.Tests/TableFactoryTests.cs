using ClosedXML.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace TableIO.ClosedXml.Tests
{
    class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Remarks { get; set; }
    }

    [TestClass]
    public class TableFactoryTests
    {
        [TestMethod]
        public void CreateXlsxReader()
        {
            using (var workbook = new XLWorkbook("files/Valid.xlsx"))
            {
                var worksheet = workbook.Worksheet(1);

                var tableReader = new TableFactory().CreateXlsxReader<Model>(worksheet, 1, 1, 4, true);
                var models = tableReader.Read().ToList();

                Assert.AreEqual(5, models.Count);
                var model = models[0];
                Assert.AreEqual(1, model.Id);
                Assert.AreEqual("name 1", model.Name);
                Assert.AreEqual(1000, model.Price);
                Assert.AreEqual("remarks1", model.Remarks);
            }
        }

        [TestMethod]
        public void CreateXlsxWriter()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("sample");

                var models = new[]
                {
                    new Model { Id = 10, Name = "NAME_10", Price = 100, Remarks = "REMARKS_10" },
                    new Model { Id = 20, Name = "NAME_20", Price = 200, Remarks = null },
                    new Model { Id = 30, Name = "NAME_30", Price = 300, Remarks = "REMARKS_30" }
                };

                var tableWriter = new TableFactory().CreateXlsxWriter<Model>(worksheet, 1, 1);
                tableWriter.Write(models, new[] { "ID", "NAME", "PRICE", "REMARKS" });

                Assert.AreEqual("ID", worksheet.Cell(1, 1).Value);
                Assert.AreEqual("REMARKS", worksheet.Cell(1, 4).Value);

                Assert.AreEqual(10D, worksheet.Cell(2, 1).Value);
                Assert.AreEqual("REMARKS_10", worksheet.Cell(2, 4).Value);

                Assert.AreEqual("", worksheet.Cell(3, 4).Value);

                Assert.AreEqual(300D, worksheet.Cell(4, 3).Value);
            }
        }

        [TestMethod]
        public void CreateXlsxWriter_NoHeader()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("sample");

                var models = new[]
                {
                    new Model { Id = 10, Name = "NAME_10", Price = 100, Remarks = "REMARKS_10"
                    },
                    new Model { Id = 20, Name = "NAME_20", Price = 200, Remarks = null
                    },
                    new Model { Id = 30, Name = "NAME_30", Price = 300, Remarks = "REMARKS_30"
                    }
                };

                var tableWriter = new TableFactory().CreateXlsxWriter<Model>(worksheet, 1, 1);
                tableWriter.Write(models);

                Assert.AreEqual(10D, worksheet.Cell(1, 1).Value);
                Assert.AreEqual("REMARKS_10", worksheet.Cell(1, 4).Value);
            }
        }
    }
}
