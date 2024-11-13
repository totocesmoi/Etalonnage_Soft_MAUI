using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class Pagination<T>
    {
        public long TotalCount { get; set; }
        public int Index { get; set; }
        public int Cout { get; set; }
        public IEnumerable<T> Items { get; set; } = null!;
    }
}
