using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class FilterAggregate
    {
        public int MinPrice { get; set; } = 0;
        public int MaxPrice { get; set; } = 100000;
        public List<string>? Categories { get; set; }
        public List<string>? Manufacturers { get; set; }
    }
}
