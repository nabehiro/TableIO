using Microsoft.VisualStudio.TestTools.UnitTesting;
using TableIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableIO.PropertyMappers;

namespace TableIO.Tests
{
    [TestClass()]
    public class AutoIndexPropertyMapperTests
    {
        class Model
        {
            public int P1 { get; set; }
            public int P2 { get; set; }
            public int P3 { get; set; }
        }

        [TestMethod()]
        public void CreatePropertyMaps()
        {
            var mapper = new AutoIndexPropertyMapper();
            var maps = mapper.CreatePropertyMaps(typeof(Model), null).ToArray();

            Assert.AreEqual(0, maps[0].ColumnIndex);
            Assert.AreEqual(1, maps[1].ColumnIndex);
            Assert.AreEqual(2, maps[2].ColumnIndex);

            Assert.AreEqual("P1", maps[0].Property.Name);
            Assert.AreEqual("P2", maps[1].Property.Name);
            Assert.AreEqual("P3", maps[2].Property.Name);
        }
    }
}