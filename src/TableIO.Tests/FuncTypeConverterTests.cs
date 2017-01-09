using Microsoft.VisualStudio.TestTools.UnitTesting;
using TableIO.TypeConverters;

namespace TableIO.Tests
{
    [TestClass()]
    public class FuncTypeConverterTests
    {
        [TestMethod()]
        public void ConvertFromField()
        {
            var converter = new FuncTypeConverter
            {
                ConvertFromFieldFunc = s => (s as string) == "押忍",
                ConvertToFieldFunc = v => (bool)v ? "押忍" : null
            };

            var val = converter.ConvertFromField("押忍");
            Assert.AreEqual(true, val);
            val = converter.ConvertFromField("御意");
            Assert.AreEqual(false, val);

            var str = converter.ConvertToField(true);
            Assert.AreEqual("押忍", str);
            str = converter.ConvertToField(false);
            Assert.IsNull(str);
        }
    }
}