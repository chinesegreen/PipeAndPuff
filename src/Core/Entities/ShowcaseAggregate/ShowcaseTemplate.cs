using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.ShowcaseAggregate
{
    public class ShowcaseTemplate : BaseEntity
    {
        public ShowcaseBlock? Main { get; set; }
        public ShowcaseBlock? Mone { get; set; }
        public ShowcaseBlock? Mtwo { get; set; }
        public ShowcaseBlock? Sone { get; set; }
        public ShowcaseBlock? Stwo { get; set; }
        public ShowcaseBlock? Sthree { get; set; }
        public ShowcaseBlock? Sfour { get; set; }
    }
}
