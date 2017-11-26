using System;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TableIO.PropertyMappers;
using TableIO.TypeConverters;

namespace TableIO.Tests
{
    [TestClass()]
    public class ManualIndexPropertyMapperTests
    {
        class Model
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Remarks { get; set; }
            public DateTime DateTime { get; set; }
        }

        [TestMethod()]
        public void CreatePropertyMapsTest()
        {
            var mapper = new ManualIndexPropertyMapper<Model>()
                .Map(m => m.Id, 11)
                .Map(m => m.Name,10)
                .Map(m => m.Price, 1)
                .Map(m => m.Remarks, 0)
                .Map(x=> x.DateTime, 5, new DefaultTypeConverter(new System.ComponentModel.DateTimeConverter()))
                ;

            var maps = mapper.CreatePropertyMaps(typeof(Model), null);

            Assert.AreEqual("Id", maps[0].Property.Name);
            Assert.AreEqual(11, maps[0].ColumnIndex);
            Assert.IsNull(maps[0].TypeConverter);

            Assert.AreEqual("Name", maps[1].Property.Name);
            Assert.AreEqual(10, maps[1].ColumnIndex);
            Assert.IsNull(maps[1].TypeConverter);

            Assert.AreEqual("Price", maps[2].Property.Name);
            Assert.AreEqual(1, maps[2].ColumnIndex);
            Assert.IsNull(maps[2].TypeConverter);

            Assert.AreEqual("Remarks", maps[3].Property.Name);
            Assert.AreEqual(0, maps[3].ColumnIndex);
            Assert.IsNull(maps[3].TypeConverter);

            Assert.AreEqual("DateTime", maps[4].Property.Name);
            Assert.AreEqual(5, maps[4].ColumnIndex);
            Assert.AreEqual(typeof(DefaultTypeConverter), maps[4].TypeConverter.GetType());
        }
    }
}