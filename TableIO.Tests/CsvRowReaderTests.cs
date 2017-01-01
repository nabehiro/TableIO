using Microsoft.VisualStudio.TestTools.UnitTesting;
using TableIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TableIO.Tests
{
    [TestClass()]
    public class CsvRowReaderTests
    {
        [TestMethod()]
        public void ReadTest()
        {
            var sr = new StringReader("aaa,aaa,aaa\r\nbbb,bbb,bbb\r\n");
            var reader = new CsvRowReader(sr);

            CollectionAssert.AreEqual(new[] { "aaa", "aaa", "aaa" }, reader.Read().ToArray());
            CollectionAssert.AreEqual(new[] { "bbb", "bbb", "bbb" }, reader.Read().ToArray());
            Assert.IsNull(reader.Read());
        }

        [TestMethod]
        public void ReadNothingTest()
        {
            var sr = new StringReader("");
            var reader = new CsvRowReader(sr);

            Assert.IsNull(reader.Read());
        }

        [TestMethod]
        public void ReadEmptyTest()
        {
            var sr = new StringReader(",,\n,,\r\n,");
            var reader = new CsvRowReader(sr);

            CollectionAssert.AreEqual(new[] { "", "", "" }, reader.Read().ToArray());
            CollectionAssert.AreEqual(new[] { "", "", "" }, reader.Read().ToArray());
            CollectionAssert.AreEqual(new[] { "", "" }, reader.Read().ToArray());
            Assert.IsNull(reader.Read());
        }

        [TestMethod]
        public void ReadEscapedTest()
        {
            var sr = new StringReader($@"""b"",""
b"",""""""b"",""""""""
""c,"","","",""{'\r'}"",""{'\n'}""
");
            var reader = new CsvRowReader(sr);

            CollectionAssert.AreEqual(new[] { "b", "\r\nb", "\"b", "\"" }, reader.Read().ToArray());
            CollectionAssert.AreEqual(new[] { "c,", ",", "\r", "\n" }, reader.Read().ToArray());
            Assert.IsNull(reader.Read());
        }

    }
}