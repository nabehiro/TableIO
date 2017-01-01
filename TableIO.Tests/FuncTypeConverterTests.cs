using Microsoft.VisualStudio.TestTools.UnitTesting;
using TableIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TableIO.Tests
{
    [TestClass()]
    public class FuncTypeConverterTests
    {
        [TestMethod()]
        public void ConvertFromString()
        {
            var converter = new FuncTypeConverter
            {
                ConvertFromStringFunc = s => s == "押忍",
                ConvertToStringFunc = v => (bool)v ? "押忍" : null
            };

            var val = converter.ConvertFromString("押忍");
            Assert.AreEqual(true, val);
            val = converter.ConvertFromString("御意");
            Assert.AreEqual(false, val);

            var str = converter.ConvertToString(true);
            Assert.AreEqual("押忍", str);
            str = converter.ConvertToString(false);
            Assert.IsNull(str);
        }
    }
}