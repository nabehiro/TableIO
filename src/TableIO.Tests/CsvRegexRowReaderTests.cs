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
    public class CsvRegexRowReaderTests
    {
        [TestMethod()]
        public void Read()
        {
            var sr = new StringReader("aaa,aaa,aaa\r\nbbb,bbb,bbb\r\n");
            var reader = new CsvRegexRowReader(sr);

            CollectionAssert.AreEqual(new[] { "aaa", "aaa", "aaa" }, reader.Read().ToArray());
            CollectionAssert.AreEqual(new[] { "bbb", "bbb", "bbb" }, reader.Read().ToArray());
            Assert.IsNull(reader.Read());
        }

        [TestMethod]
        public void ReadNothing()
        {
            var sr = new StringReader("");
            var reader = new CsvRegexRowReader(sr);

            Assert.IsNull(reader.Read());
        }

        [TestMethod]
        public void ReadEmpty()
        {
            var sr = new StringReader(",,\n,,\r\n,");
            var reader = new CsvRegexRowReader(sr);

            CollectionAssert.AreEqual(new[] { "", "", "" }, reader.Read().ToArray());
            CollectionAssert.AreEqual(new[] { "", "", "" }, reader.Read().ToArray());
            CollectionAssert.AreEqual(new[] { "", "" }, reader.Read().ToArray());
            Assert.IsNull(reader.Read());
        }

        [TestMethod]
        public void ReadEscaped()
        {
            var sr = new StringReader($@"""b"",""
b"",""""""b"",""""""""
""c,"","","",""{'\r'}"",""{'\n'}""
");
            var reader = new CsvRegexRowReader(sr);

            CollectionAssert.AreEqual(new[] { "b", "\r\nb", "\"b", "\"" }, reader.Read().ToArray());
            CollectionAssert.AreEqual(new[] { "c,", ",", "\r", "\n" }, reader.Read().ToArray());
            Assert.IsNull(reader.Read());
        }

    }
}