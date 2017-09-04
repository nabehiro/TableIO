using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using TableIO.RowWriters;

namespace TableIO.Tests
{
    [TestClass()]
    public class TsvRowWriterTests
    {
        [TestMethod()]
        public void Write()
        {
            var sw = new StringWriter();
            var writer = new TsvRowWriter(sw);

            writer.Write(new[] { "aaa", "aaa", "aaa" });
            writer.Write(new[] { "bbb", "bbb", "bbb" });

            Assert.AreEqual("aaa\taaa\taaa\r\nbbb\tbbb\tbbb\r\n", sw.ToString());
        }

        [TestMethod]
        public void WriteEmpty()
        {
            var sw = new StringWriter();
            var writer = new TsvRowWriter(sw);

            writer.Write(new[] { "", "", "" });
            writer.Write(new string [] { null, null, null });

            Assert.AreEqual("\t\t\r\n\t\t\r\n", sw.ToString());
        }

        [TestMethod]
        public void WriteEscape()
        {
            var sw = new StringWriter();
            var writer = new TsvRowWriter(sw);

            writer.Write(new[] { "\t", "a\t", "\r", "\n", "\"" });
            // "\t"\t"a\t"\t"\r"\t"\n"\t""""
            Assert.AreEqual("\"\t\"\t\"a\t\"\t\"\r\"\t\"\n\"\t\"\"\"\"\r\n", sw.ToString());
        }

        [TestMethod]
        public void WriteNothing()
        {
            var sw = new StringWriter();
            var writer = new TsvRowWriter(sw);

            Assert.AreEqual("", sw.ToString());
        }

        [TestMethod]
        public void WriteNoFields()
        {
            var sw = new StringWriter();
            var writer = new TsvRowWriter(sw);

            writer.Write(new string[] { });
            writer.Write(new string[] { });

            Assert.AreEqual("\r\n\r\n", sw.ToString());
        }
    }
}