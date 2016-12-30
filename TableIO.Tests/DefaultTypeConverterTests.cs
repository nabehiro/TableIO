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
    public class DefaultTypeConverterTests
    {
        class Model
        {
            public int PInt { get; set; }
            public string PString { get; set; }
        }

        private TypeConverter GetPropetyConverter(string name)
        {
            var props = TypeDescriptor.GetProperties(typeof(Model));
            return props.Find(name, true).Converter;
        }

        [TestMethod()]
        public void ConvertFromString_Int()
        {
            var propConverter = GetPropetyConverter("PInt");
            var converter = new DefaultTypeConverter(propConverter);
            
            var val = converter.ConvertFromString("1");
            Assert.AreEqual(1, val);

            //val = converter.ConvertFromString(long.MaxValue.ToString());
            // => error
            //val = converter.ConvertFromString("");
            // => error
            //val = converter.ConvertFromString(null);
            // => error
        }

        [TestMethod]
        public void ConvertToString_Int()
        {
            var propConverter = GetPropetyConverter("PInt");
            var converter = new DefaultTypeConverter(propConverter);

            var str = converter.ConvertToString(1);
        }

        [TestMethod()]
        public void ConvertFromString_String()
        {
            var propConverter = GetPropetyConverter("PString");
            var converter = new DefaultTypeConverter(propConverter);

            var val = converter.ConvertFromString("abc");
            Assert.AreEqual("abc", val);

            //val = converter.ConvertFromString(null);
            // ==> val = ""
        }
    }
}