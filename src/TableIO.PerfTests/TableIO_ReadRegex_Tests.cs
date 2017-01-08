using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Diagnostics;

namespace TableIO.PerfTests
{
    [TestClass]
    public class TableIO_ReadRegex_Tests
    {
        class Model
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Remarks { get; set; }
        }

        [TestMethod]
        public void Regex_Read_Unescaped_10000()
        {
            var sw = Stopwatch.StartNew();

            using (var stm = new StreamReader("files\\data_unescaped_10000.csv"))
            {
                var reader = new TableFactory().CreateReader<Model>(new CsvRegexRowReader(stm));
                var models = reader.Read();

                Assert.AreEqual(10000, models.Count);
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        [TestMethod]
        public void Regex_Read_Escaped_10000()
        {
            var sw = Stopwatch.StartNew();

            using (var stm = new StreamReader("files\\data_escaped_10000.csv"))
            {
                var reader = new TableFactory().CreateReader<Model>(new CsvRegexRowReader(stm));
                var models = reader.Read();

                Assert.AreEqual(10000, models.Count);
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        [TestMethod]
        public void Regex_Read_Mixed_10000()
        {
            var sw = Stopwatch.StartNew();

            using (var stm = new StreamReader("files\\data_mixed_10000.csv"))
            {
                var reader = new TableFactory().CreateReader<Model>(new CsvRegexRowReader(stm));
                var models = reader.Read();

                Assert.AreEqual(10000, models.Count);
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        [TestMethod]
        public void Regex_Read_Mixed_100000()
        {
            var sw = Stopwatch.StartNew();

            using (var stm = new StreamReader("files\\data_mixed_100000.csv"))
            {
                var reader = new TableFactory().CreateReader<Model>(new CsvRegexRowReader(stm));
                var models = reader.Read();

                Assert.AreEqual(100000, models.Count);
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        [TestMethod]
        public void Regex_Read_Rand_Str_10000()
        {
            var sw = Stopwatch.StartNew();

            using (var stm = new StreamReader("files\\data_rand_str_10000.csv"))
            {
                var reader = new TableFactory().CreateReader<Model>(new CsvRegexRowReader(stm));
                var models = reader.Read();

                Assert.AreEqual(10000, models.Count);
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}
