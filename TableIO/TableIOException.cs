using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableIO
{
    public class TableIOException : Exception
    {
        public TableIOException() { }
        public TableIOException(string message) : base(message) { }
        public TableIOException(string message, Exception innerException) : base(message, innerException) { }
    }
}
