using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableIO
{
    public class TableIOException : Exception
    {
        public IList<ErrorDetail> Errors { get; set; }

        public TableIOException() { }
        public TableIOException(string message) : base(message) { }
        public TableIOException(string message, Exception innerException) : base(message, innerException) { }

        public TableIOException(IList<ErrorDetail> errors)
            : base($"{errors.FirstOrDefault()?.ToString()}" +
                  ( errors.Count == 0 ? "" 
                  : errors.Count == 1 ? " and 1 other error." 
                  : $" and {errors.Count - 1} other errors." ))
        {
            Errors = errors;
            for (int i = 0; i < errors.Count; i++)
                Data[i] = errors[i];
        }
    }
}
