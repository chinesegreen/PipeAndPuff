using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.S3Models
{
    public class UploadingResult
    {
        public S3ResponseDto Response { get; set; }
        public string FileName { get; set; }
    }
}
