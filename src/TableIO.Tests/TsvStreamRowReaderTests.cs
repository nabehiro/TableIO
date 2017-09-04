using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using TableIO.RowReaders;

namespace TableIO.Tests
{
    [TestClass()]
    public class TsvStreamRowReaderTests
    {
        private TsvStreamRowReader CreateReader(TextReader textReader)
        {
            return new TsvStreamRowReader(textReader, 2);
        }

        [TestMethod()]
        public void Read()
        {
            var sr = new StringReader("aaa\taaa\taaa\r\nbbb\tbbb\tbbb\r\n");
            var reader = CreateReader(sr);

            CollectionAssert.AreEqual(new[] { "aaa", "aaa", "aaa" }, reader.Read().ToArray());
            CollectionAssert.AreEqual(new[] { "bbb", "bbb", "bbb" }, reader.Read().ToArray());
            Assert.IsNull(reader.Read());
        }

        [TestMethod]
        public void ReadNothing()
        {
            var sr = new StringReader("");
            var reader = CreateReader(sr);

            Assert.IsNull(reader.Read());
        }

        [TestMethod]
        public void ReadEmpty()
        {
            var sr = new StringReader("\t\t\n\t\t\r\n\t");
            var reader = CreateReader(sr);

            CollectionAssert.AreEqual(new[] { "", "", "" }, reader.Read().ToArray());
            CollectionAssert.AreEqual(new[] { "", "", "" }, reader.Read().ToArray());
            CollectionAssert.AreEqual(new[] { "", "" }, reader.Read().ToArray());
            Assert.IsNull(reader.Read());
        }

        [TestMethod]
        public void ReadEscaped()
        {
            var sr = new StringReader($@"""""""""
""b""{'\t'}""
b""{'\t'}""""""b""{'\t'}""""""""
""c{'\t'}""{'\t'}""{'\t'}""{'\t'}""{'\r'}""{'\t'}""{'\n'}""
");
            var reader = CreateReader(sr);

            CollectionAssert.AreEqual(new[] { "\"" }, reader.Read().ToArray());
            CollectionAssert.AreEqual(new[] { "b", "\r\nb", "\"b", "\"" }, reader.Read().ToArray());
            CollectionAssert.AreEqual(new[] { "c\t", "\t", "\r", "\n" }, reader.Read().ToArray());
            Assert.IsNull(reader.Read());
        }

        
    }
}