using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableIO
{
    public class ErrorDetail
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public int? RowIndex { get; set; }
        public int? ColumnIndex { get; set; }
        public IEnumerable<string> MemberNames { get; set; }

        public override string ToString()
        {
            var additional = new StringBuilder();

            if (!string.IsNullOrEmpty(Type))
                additional.Append($"Type:{Type}, ");
            if (RowIndex.HasValue)
                additional.Append($"RowIndex:{RowIndex}, ");
            if (ColumnIndex.HasValue)
                additional.Append($"ColumnIndex:{ColumnIndex}, ");
            if (MemberNames?.Any() == true)
                additional.Append($"MemberNames:{string.Join(",", MemberNames)}, ");

            if (additional.Length > 0)
            {
                additional.Remove(additional.Length - 2, 2)
                    .Insert(0, '(').Append(')');
            }

            return $"{Message}{additional.ToString()}";
        }
    }
}
