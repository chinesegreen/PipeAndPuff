using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Options
{
    public class AwsCredentials
    {
        public string AwsKey { get; set; } = string.Empty;
        public string AwsSecretKey { get; set; } = string.Empty;
    }
}
