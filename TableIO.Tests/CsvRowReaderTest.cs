using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace TableIO.Tests
{
    [TestClass]
    public class CsvRowReaderTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var text = @"""a"",""b,\r\n"",c,""""""""
1,2,3
10,11,12
";
            text = "a,\"1\",\"\r\n\",b";



        var reader = new CsvRowReader { TextReader = new StringReader(text) };

            var row1 = reader.Read();
            var row2 = reader.Read();
            var row3 = reader.Read();
            var row4 = reader.Read();

        }

        public class TestClass
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public TestClass MyProperty { get; set; }
        }

        [TestMethod]
        public void TestMethod2()
        {
            //var props = TypeDescriptor.GetProperties(typeof(TestClass)).OfType<PropertyDescriptor>().ToArray();
            var x = GetProertyName(t => t.Id);
        }

        public string GetProertyName<TProperty>(Expression<Func<TestClass, TProperty>> expression)
        {
            return ((MemberExpression)expression.Body).Member.Name;
        }
    }
}
