using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace TableIO.RowWriters
{
    public class TsvRowWriter : IRowWriter
    {
        public TextWriter TextWriter { get; }

        private static readonly Regex _escapeCharRegex = new Regex("\\t|\"|\\r|\\n", RegexOptions.Compiled);

        public TsvRowWriter(TextWriter textWriter)
        {
            TextWriter = textWriter;
        }

        public void Write(IList<object> row)
        {
            if (row.Count == 0)
            {
                TextWriter.WriteLine();
                return;
            }

            var sb = new StringBuilder();
            foreach (var field in row)
            {
                if (field == null)
                {
                    sb.Append("\t");
                }
                else
                {
                    var val = (field as string) ?? $"{field}";
                    if (val != "")
                    {
                        if (_escapeCharRegex.IsMatch(val))
                            sb.Append($"\"{val.Replace("\"", "\"\"")}\"\t");
                        else
                            sb.Append($"{val}\t");
                    }
                    else
                    {
                        sb.Append("\t");
                    }
                }
            }

            sb.Remove(sb.Length - 1, 1);

            TextWriter.WriteLine(sb.ToString());
        }
    }
}
