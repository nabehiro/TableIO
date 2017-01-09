using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableIO.RowReaders
{
    public interface IRowReader
    {
        IList<object> Read();
    }
}
