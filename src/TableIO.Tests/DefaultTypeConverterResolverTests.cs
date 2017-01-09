using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using TableIO.TypeConverters;

namespace TableIO.Tests
{
    [TestClass()]
    public class DefaultTypeConverterResolverTests
    {
        class Model
        {
            public int PInt { get; set; }
            public string PString { get; set; }
        }

        private PropertyDescriptor GetPropety(string name)
        {
            var props = TypeDescriptor.GetProperties(typeof(Model));
            return props.Find(name, true);
        }

        [TestMethod()]
        public void GetTypeConverter()
        {
            var resolver = new DefaultTypeConverterResolver<Model>();
            var prop = GetPropety("PInt");

            var converter = resolver.GetTypeConverter(prop);
            Assert.IsInstanceOfType(converter, typeof(DefaultTypeConverter));

            var converter2 = resolver.GetTypeConverter(prop);
            Assert.AreEqual(converter, converter2);
        }

        [TestMethod()]
        public void SetTypeConverter()
        {
            var resolver = new DefaultTypeConverterResolver<Model>();
            var prop = GetPropety("PInt");

            var converter = resolver.GetTypeConverter(prop);
            Assert.IsInstanceOfType(converter, typeof(DefaultTypeConverter));

            resolver.SetTypeConverter(m => m.PInt, new FuncTypeConverter());

            converter = resolver.GetTypeConverter(prop);
            Assert.IsInstanceOfType(converter, typeof(FuncTypeConverter));

        }
    }
}