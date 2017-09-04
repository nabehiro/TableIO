using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TableIO.PropertyMappers;

namespace TableIO.Tests
{
    [TestClass]
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
        public void ReadCsv()
        {
            using (var stmReader = new StreamReader("files\\Valid.csv"))
            {
                var csvReader = new TableFactory().CreateCsvReader<Model>(stmReader, true);
                var models = csvReader.Read().ToList();

                Assert.AreEqual(5, models.Count);
                
                var model = models[3];
                Assert.AreEqual(4, model.Id);
                Assert.AreEqual("name \"4\"", model.Name);
                Assert.AreEqual(4000, model.Price);
                Assert.AreEqual("remarks4\r\nremarks4", model.Remarks);
            }
        }

        [TestMethod]
        public void ReadTsv()
        {
            using (var stmReader = new StreamReader("files\\Valid.tsv"))
            {
                var csvReader = new TableFactory().CreateTsvReader<Model>(stmReader, true);
                var models = csvReader.Read().ToList();

                Assert.AreEqual(5, models.Count);

                var model = models[3];
                Assert.AreEqual(4, model.Id);
                Assert.AreEqual("name \"4\"", model.Name);
                Assert.AreEqual(4000, model.Price);
                Assert.AreEqual("remarks4\r\nremarks4", model.Remarks);
            }
        }

        [TestMethod]
        public void ReadCsvYield()
        {
            using (var stmReader = new StreamReader("files\\Valid.csv"))
            {
                var csvReader = new TableFactory().CreateCsvReader<Model>(stmReader, true);

                var models = new List<Model>();
                foreach (var m in csvReader.Read())
                    models.Add(m);

                Assert.AreEqual(5, models.Count);

                var model = models[3];
                Assert.AreEqual(4, model.Id);
                Assert.AreEqual("name \"4\"", model.Name);
                Assert.AreEqual(4000, model.Price);
                Assert.AreEqual("remarks4\r\nremarks4", model.Remarks);
            }
        }

        [TestMethod]
        public void ReadCsv_ManualIndex()
        {
            var mapper = new ManualIndexPropertyMapper<Model>()
                .Map(m => m.Id, 2)
                .Map(m => m.Name, 3);

            using (var stmReader = new StreamReader("files\\Valid.csv"))
            {
                var csvReader = new TableFactory().CreateCsvReader<Model>(stmReader, true, propertyMapper:mapper);
                var models = csvReader.Read().ToList();

                Assert.AreEqual(5, models.Count);

                var model = models[3];

                Assert.AreEqual(4000, model.Id);
                Assert.AreEqual("remarks4\r\nremarks4", model.Name);
                // no set
                Assert.AreEqual(0, model.Price);
                Assert.AreEqual(null, model.Remarks);
            }
        }

        [TestMethod]
        public void CopyCsv()
        {
            IList<Model> models = null;
            using (var stmReader = new StreamReader("files\\Valid.csv"))
            {
                var csvReader = new TableFactory().CreateCsvReader<Model>(stmReader, true);
                models = csvReader.Read().ToList();
            }

            using (var stmWriter = new StreamWriter("files\\CopyValid.csv"))
            {
                var csvWriter = new TableFactory().CreateCsvWriter<Model>(stmWriter);
                csvWriter.Write(models, new[] { "ID", "NAME", "PRICE", "REMARKS" });
            }
        }

        [TestMethod]
        public void WriteCsv_ManualIndex()
        {
            var models = new[]
            {
                new Model { Id = 1, Name = "NAME_1", Price = 100, Remarks = "REMARKS_1" },
                new Model { Id = 2, Name = "NAME_2", Price = 200, Remarks = "REMARKS_2" },
            };

            var mapper = new ManualIndexPropertyMapper<Model>()
                .Map(m => m.Id, 2)
                .Map(m => m.Name, 3);

            var strWriter = new StringWriter();
            var csvReader = new TableFactory().CreateCsvWriter<Model>(strWriter, propertyMapper: mapper);
            csvReader.Write(models);

            Assert.AreEqual(",,1,NAME_1\r\n,,2,NAME_2\r\n", strWriter.ToString());
        }

        [TestMethod]
        public void CopyTsv()
        {
            IList<Model> models = null;
            using (var stmReader = new StreamReader("files\\Valid.tsv"))
            {
                var csvReader = new TableFactory().CreateTsvReader<Model>(stmReader, true);
                models = csvReader.Read().ToList();
            }

            using (var stmWriter = new StreamWriter("files\\CopyValid.tsv"))
            {
                var csvWriter = new TableFactory().CreateTsvWriter<Model>(stmWriter);
                csvWriter.Write(models, new[] { "ID", "NAME", "PRICE", "REMARKS" });
            }
        }
    }
}
