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



        public string[] Read()
        {
            //var c = TextReader.Read();
            //if (c == -1)
            //    return null;

            //var fields = new List<object>();
            //var fieldStart = 0;
            //var fieldEnd = 0;
            //var inQuote = false;
            //while (true)
            //{
            //    if (c == '"')
            //        TextReader.po
            //}

            return null;
        }
    }
}
