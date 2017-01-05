using Microsoft.VisualStudio.TestTools.UnitTesting;
using TableIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableIO.Tests
{
    [TestClass()]
    public class ErrorDetailTests
    {
        [TestMethod()]
        public void ToStringTest()
        {
            var error1 = new ErrorDetail
            {
                Message = "MESSAGE"
            };
            Assert.AreEqual("MESSAGE", error1.ToString());

            var error2 = new ErrorDetail
            {
                Message = "MESSAGE",
                Type = "TYPE",
                RowIndex = 10,
                ColumnIndex = 20,
                MemberNames = new[] { "NAME1", "NAME2" }
            };
            Assert.AreEqual("MESSAGE(Type:TYPE, RowIndex:10, ColumnIndex:20, MemberNames:NAME1,NAME2)", error2.ToString());
        }
    }
}