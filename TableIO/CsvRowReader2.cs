using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableIO
{
    public class CsvRowReader2 : IRowReader
    {
        public TextReader TextReader { get; set; }

        private char[] _buffer = new char[4];

        private int curBufferSize = 0;
        private int curBufferPosition = 0;

        public char ReadChar()
        {
            if (curBufferPosition >= curBufferSize)
            {
                curBufferPosition = 0;
                curBufferSize = TextReader.Read(_buffer, 0, _buffer.Length);
                if (curBufferSize == 0)
                    return '\0';
            }
            //new string()
            return _buffer[curBufferPosition++];
        }



        public IList<string> Read()
        {
            return null;
        }
    }
}
