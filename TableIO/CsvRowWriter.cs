using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TableIO
{
    public class CsvRowWriter : IRowWriter
    {
        public TextWriter TextWriter { get; set; }

        private static readonly Regex _escapeCharRegex = new Regex(",|\"|\\r|\\n", RegexOptions.Compiled);

        public void Write(IList<string> row)
        {
            if (row.Count == 0)
            {
                TextWriter.WriteLine();
                return;
            }

            var sb = new StringBuilder();
            foreach (var field in row)
            {
                if (!string.IsNullOrEmpty(field))
                {
                    if (_escapeCharRegex.IsMatch(field))
                        sb.Append($"\"{field.Replace("\"", "\"\"")}\",");
                    else
                        sb.Append($"{field},");
                }
                else
                    sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);

            TextWriter.WriteLine(sb.ToString());
        }
    }
}
