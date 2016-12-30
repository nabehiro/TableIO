﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TableIO
{
    public class CsvRowReader : IRowReader
    {
        public TextReader TextReader { get; set; }

        private string _text = null;

        // , | line break | non-escaped | escaped
        private static readonly Regex _tokenizer = new Regex(",|\\r?\\n|[^,\"\\r\\n]+|\"(?:[^\"]|\"\")*\"", RegexOptions.Compiled);
        private static readonly Regex _trimEndRegex = new Regex("\r?\n$", RegexOptions.Compiled);
        private Match _match = null;

        public IList<string> Read()
        {
            if (_text == null)
            {
                _text = _trimEndRegex.Replace(TextReader.ReadToEnd() ?? "", "");
                _match = _tokenizer.Match(_text);
            }

            if (!_match.Success)
                return null;

            var fields = new List<string> { "" };

            while (_match.Success)
            {
                var val = _match.Value;
                _match = _match.NextMatch();

                switch (val)
                {
                    case ",":
                        fields.Add("");
                        break;
                    case "\n":
                    case "\r\n":
                        return fields;
                    default:
                        fields[fields.Count - 1] = val.StartsWith("\"") ?
                            val.Substring(1, val.Length - 2).Replace("\"\"", "\"") : val;
                        break;
                }
            }

            return fields;
        }
    }
}
