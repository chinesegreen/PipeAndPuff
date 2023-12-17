using Amazon.S3;
using Amazon.S3.Model;
using Infrastructure.Options;
using Infrastructure.S3Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IStorageService
    {
        public Task<S3ResponseDto> UploadFileAsync(S3Models.S3Object s3object);
    }
}
