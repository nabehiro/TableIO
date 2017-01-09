using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using TableIO.TypeConverters;

namespace TableIO.Tests
{
    [TestClass()]
    public class DefaultTypeConverterTests
    {
        class Model
        {
            public int PInt { get; set; }
            public string PString { get; set; }
            public int? PNInt { get; set; }
        }

        private TypeConverter GetPropetyConverter(string name)
        {
            var props = TypeDescriptor.GetProperties(typeof(Model));
            return props.Find(name, true).Converter;
        }

        [TestMethod()]
        public void ConvertFromField_Int()
        {
            var propConverter = GetPropetyConverter("PInt");
            var converter = new DefaultTypeConverter(propConverter);
            
            var val = converter.ConvertFromField("1");
            Assert.AreEqual(1, val);

            //val = converter.ConvertFromField(long.MaxValue.ToField());
            // => error
            //val = converter.ConvertFromField("");
            // => error
            //val = converter.ConvertFromField(null);
            // => error
        }

        [TestMethod]
        public void ConvertToField_Int()
        {
            var propConverter = GetPropetyConverter("PInt");
            var converter = new DefaultTypeConverter(propConverter);

            var fieldVal = converter.ConvertToField(1);
            Assert.AreEqual(1, fieldVal);
        }

        [TestMethod()]
        public void ConvertFromField_NullableInt()
        {
            var propConverter = GetPropetyConverter("PNInt");
            var converter = new DefaultTypeConverter(propConverter);

            var val = converter.ConvertFromField("1");
            Assert.AreEqual(1, val);

            val = converter.ConvertFromField("");
            Assert.IsNull(val);

            val = converter.ConvertFromField(null);
            Assert.IsNull(val);

            //val = converter.ConvertFromField(long.MaxValue.ToField());
            // => error
        }

        [TestMethod()]
        public void ConvertFromField_String()
        {
            var propConverter = GetPropetyConverter("PString");
            var converter = new DefaultTypeConverter(propConverter);

            var val = converter.ConvertFromField("abc");
            Assert.AreEqual("abc", val);

            val = converter.ConvertFromField(null);
            Assert.AreEqual("", val);
        }
    }
}