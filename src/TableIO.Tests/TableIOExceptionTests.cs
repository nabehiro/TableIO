using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TableIO.Tests
{
    [TestClass()]
    public class TableIOExceptionTests
    {
        [TestMethod()]
        public void TableIOException()
        {
            var errors = new[]
            {
                new ErrorDetail { Message = "MSG1" }
            };
            var ex = new TableIOException(errors);
            Assert.AreEqual("MSG1", ex.Message);
            Assert.AreEqual(errors[0], ex.Data[0]);


            errors = new[]
            {
                new ErrorDetail { Message = "MSG1" },
                new ErrorDetail { Message = "MSG2" },
            };
            ex = new TableIOException(errors);
            Assert.AreEqual("MSG1 and 1 other error.", ex.Message);
            Assert.AreEqual(errors[1], ex.Data[1]);


            errors = new[]
            {
                new ErrorDetail { Message = "MSG1" },
                new ErrorDetail { Message = "MSG2" },
                new ErrorDetail { Message = "MSG3" },
            };
            ex = new TableIOException(errors);
            Assert.AreEqual("MSG1 and 2 other errors.", ex.Message);
        }
    }
}