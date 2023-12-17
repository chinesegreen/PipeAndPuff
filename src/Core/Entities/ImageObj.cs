using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ImageObj
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public int ProductId { get; set; }
        public string Image { get; set; }
    }
}
