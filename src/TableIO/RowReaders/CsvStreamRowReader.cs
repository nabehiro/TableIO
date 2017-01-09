using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TableIO.RowReaders
{
    public class CsvStreamRowReader : IRowReader
    {
        public TextReader TextReader { get; set; }

        private readonly int _maxBufferSize;
        private readonly char[] _buffer;
        private int _bufferSize = 0;
        private int _bufferPosition = 0;
        private int _startFieldPosition = -1;
        private string _tempField = "";
        private readonly List<object> _fields = new List<object>();

        public CsvStreamRowReader(TextReader textReader, int maxBufferSize = 2048)
        {
            TextReader = textReader;
            _maxBufferSize = maxBufferSize;
            _buffer = new char[_maxBufferSize];
        }

        public IList<object> Read()
        {
            _fields.Clear();

            while(true)
            {
                var c = ScanChar();
                switch (c)
                {
                    case '\r':
                        c = ScanChar();
                        if (c != '\n')
                            throw new InvalidOperationException("Text contains unexpected carriage return('\\n').");
                        if (!_fields.Any())
                            return null;
                        _fields.Add("");
                        return _fields;
                    case '\n':
                    case '\0':
                        if (!_fields.Any())
                            return null;
                        _fields.Add("");
                        return _fields;
                    case ',':
                        _fields.Add("");
                        break;
                    case '"':
                        // process "AAA",
                        if (!ScanEscapedField())
                            return _fields;
                        break;
                    default:
                        // process AAA,
                        if (!ScanField())
                            return _fields;
                        break;
                }
            }
        }

        private char ScanChar()
        {
            if (_bufferPosition >= _bufferSize)
            {
                if (_startFieldPosition != -1)
                {
                    _tempField += new string(_buffer, _startFieldPosition, _bufferSize - _startFieldPosition);
                    _startFieldPosition = 0;
                }

                _bufferPosition = 0;
                _bufferSize = TextReader.Read(_buffer, 0, _maxBufferSize);
                if (_bufferSize == 0)
                    return '\0';
            }
            return _buffer[_bufferPosition++];
        }

        private bool Scan()
        {
                var c = ScanChar();
                switch(c)
                {
                    case '\r':
                        c = ScanChar();
                        if (c != '\n')
                            throw new InvalidDataException("Text contains unexpected carriage return('\\n').");
                        if (_fields.Any())
                            _fields.Add("");
                        return false;
                    case '\n':
                    case '\0':
                        if (_fields.Any())
                            _fields.Add("");
                        return false;
                    case ',':
                        _fields.Add("");
                        return true;
                    case '"':
                        return ScanEscapedField();
                    default:
                        return ScanField();
                }
        }
    
        private bool ScanField()
        {
            _startFieldPosition = _bufferPosition - 1;

            while(true)
            {
                var c = ScanChar();
                switch(c)
                {
                    case '\r':
                        AddField();
                        c = ScanChar();
                        if (c != '\n')
                            throw new InvalidDataException("Text contains unexpected carriage return('\\n').");
                        return false;
                    case '\n':
                    case '\0':
                        AddField();
                        return false;
                    case ',':
                        AddField();
                        return true;
                    case '"':
                        throw new InvalidDataException("Text contains unexpected double quote ('\"').");
                }
            }
        }

        private void AddField()
        {
            var length = _bufferPosition - _startFieldPosition - 1;
            _fields.Add(length > 0 ? _tempField + new string(_buffer, _startFieldPosition, length) : _tempField);

            _startFieldPosition = -1;
            _tempField = "";
        }

        private bool ScanEscapedField()
        {
            _startFieldPosition = _bufferPosition - 1;

            while (true)
            {
                var c = ScanChar();
                switch (c)
                {
                    case '\0':
                        throw new InvalidDataException("Text is not closed by double quote ('\"').");
                    case '"':
                        c = ScanChar();
                        switch(c)
                        {
                            case '"':
                                break;
                            case '\r':
                                AddEscapedField();
                                c = ScanChar();
                                if (c != '\n')
                                    throw new InvalidDataException("Text contains unexpected carriage return('\\n').");
                                return false;
                            case '\n':
                            case '\0':
                                AddEscapedField();
                                return false;
                            case ',':
                                AddEscapedField();
                                return true;
                            default:
                                throw new InvalidDataException("Text contains unexpected double quote ('\"').");
                        }
                        break;
                }
            }
        }

        private void AddEscapedField()
        {
            var length = _bufferPosition - _startFieldPosition - 1;
            var field = length > 0 ? _tempField + new string(_buffer, _startFieldPosition, length) : _tempField;
            _fields.Add(field.Substring(1, field.Length - 2).Replace("\"\"", "\""));

            _startFieldPosition = -1;
            _tempField = "";
        }
    }
}
