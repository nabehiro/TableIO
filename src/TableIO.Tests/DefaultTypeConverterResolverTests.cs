using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using System.Reflection;
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

        [TestMethod()]
        public void GetTypeConverter()
        {
            var resolver = new DefaultTypeConverterResolver<Model>();
            var prop = typeof(Model).GetProperty("PInt");

            var converter = resolver.GetTypeConverter(prop);
            Assert.IsInstanceOfType(converter, typeof(DefaultTypeConverter));

            var converter2 = resolver.GetTypeConverter(prop);
            Assert.AreEqual(converter, converter2);
        }

        [TestMethod()]
        public void SetTypeConverter()
        {
            var resolver = new DefaultTypeConverterResolver<Model>();
            var prop = typeof(Model).GetProperty("PInt");

            var converter = resolver.GetTypeConverter(prop);
            Assert.IsInstanceOfType(converter, typeof(DefaultTypeConverter));

            resolver.SetTypeConverter(prop, new FuncTypeConverter());

            converter = resolver.GetTypeConverter(prop);
            Assert.IsInstanceOfType(converter, typeof(FuncTypeConverter));

        }
    }
}