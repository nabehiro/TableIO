using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace TableIO.Tests
{
    [TestClass]
    public class TableFactoryTests
    {
        class ValidCsvModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Remarks { get; set; }
        }

        [TestMethod]
        public void ReadValidCsv()
        {
            using (var stmReader = new StreamReader("files\\Valid.csv"))
            {
                var csvReader = new TableFactory().CreateCsvReader<ValidCsvModel>(stmReader, true);
                var models = csvReader.Read();

                Assert.AreEqual(5, models.Count);

                var model = models[3];
                Assert.AreEqual(4, model.Id);
                Assert.AreEqual("name \"4\"", model.Name);
                Assert.AreEqual(4000, model.Price);
                Assert.AreEqual("remarks4\nremarks4", model.Remarks);
            }
        }

        [TestMethod]
        public void CopyValidCsv()
        {
            IList<ValidCsvModel> models = null;
            using (var stmReader = new StreamReader("files\\Valid.csv"))
            {
                var csvReader = new TableFactory().CreateCsvReader<ValidCsvModel>(stmReader, true);
                models = csvReader.Read();
            }

            using (var stmWriter = new StreamWriter("files\\CopyValid.csv"))
            {
                var csvWriter = new TableFactory().CreateCsvWriter<ValidCsvModel>(stmWriter);
                csvWriter.Write(models, new[] { "ID", "NAME", "PRICE", "REMARKS" });
            }

        }
    }
}
