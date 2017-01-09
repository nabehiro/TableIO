using System.Collections.Generic;

namespace TableIO.RowWriters
{
    public interface IRowWriter
    {
        void Write(IList<object> row);
    }
}
