using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Diagnostics;
using TableIO.RowReaders;
using System.Linq;

namespace TableIO.PerfTests
{
    [Ignore]
    [TestClass]
    public class Read_Stream_Tests
    {
        class Model
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Remarks { get; set; }
        }

        [TestMethod]
        public void Stream_Read_Unescaped_10000()
        {
            var sw = Stopwatch.StartNew();

            using (var stm = new StreamReader("files/data_unescaped_10000.csv"))
            {
                var reader = new TableFactory().CreateReader<Model>(new CsvStreamRowReader(stm));
                var models = reader.Read().ToList();

                Assert.AreEqual(10000, models.Count);
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        [TestMethod]
        public void Stream_Read_Escaped_10000()
        {
            var sw = Stopwatch.StartNew();

            using (var stm = new StreamReader("files/data_escaped_10000.csv"))
            {
                var reader = new TableFactory().CreateReader<Model>(new CsvStreamRowReader(stm));
                var models = reader.Read().ToList();

                Assert.AreEqual(10000, models.Count);
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        [TestMethod]
        public void Stream_Read_Mixed_10000()
        {
            var sw = Stopwatch.StartNew();

            using (var stm = new StreamReader("files/data_mixed_10000.csv"))
            {
                var reader = new TableFactory().CreateReader<Model>(new CsvStreamRowReader(stm));
                var models = reader.Read().ToList();

                Assert.AreEqual(10000, models.Count);
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        [TestMethod]
        public void Stream_Read_Mixed_100000()
        {
            var sw = Stopwatch.StartNew();

            using (var stm = new StreamReader("files/data_mixed_100000.csv"))
            {
                var reader = new TableFactory().CreateReader<Model>(new CsvStreamRowReader(stm));
                var models = reader.Read().ToList();

                Assert.AreEqual(100000, models.Count);
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        [TestMethod]
        public void Stream_Read_Rand_Str_10000()
        {
            var sw = Stopwatch.StartNew();

            using (var stm = new StreamReader("files/data_rand_str_10000.csv"))
            {
                var reader = new TableFactory().CreateReader<Model>(new CsvStreamRowReader(stm));
                var models = reader.Read().ToList();

                Assert.AreEqual(10000, models.Count);
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}
