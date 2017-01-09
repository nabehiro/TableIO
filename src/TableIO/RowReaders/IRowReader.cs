using System.Collections.Generic;

namespace TableIO.RowReaders
{
    public interface IRowReader
    {
        IList<object> Read();
    }
}
