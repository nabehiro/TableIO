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
    public class TableReaderTests
    {
        class Model
        {
            public int PInt { get; set; }
            public string PString { get; set; }
            public int? PNInt { get; set; }
        }

        private TableReader<TModel> CreateTableReader<TModel>(string str) 
            where TModel : new()
        {
            var strReader = new StringReader(str);
            var rowReader = new CsvRowReader(strReader);
            var typeConvResolver = new DefaultTypeConverterResolver<TModel>();
            var mapper = new AutoIndexPropertyMapper();
            var validator = new DefaultModelValidator();

            var tableReader = new TableReader<TModel>(rowReader, typeConvResolver, mapper, validator);
            return tableReader;
        }

        [TestMethod()]
        public void ConvertFromRow()
        {
            var reader = CreateTableReader<Model>("");
            var maps = new AutoIndexPropertyMapper().CreatePropertyMaps(typeof(Model), null);

            var model = reader.ConvertFromRow(new[] { "1", "abc", "10" }, 0, maps);
            Assert.AreEqual(1, model.PInt);
            Assert.AreEqual("abc", model.PString);
            Assert.AreEqual(10, model.PNInt);
        }

        [TestMethod()]
        public void Read()
        {
            // TODO:
            Assert.Fail();
        }

        
    }
}