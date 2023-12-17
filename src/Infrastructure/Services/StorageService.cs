using Amazon.Runtime;
using Amazon.Runtime.Internal.Auth;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Infrastructure.S3Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class StorageService : IStorageService
    {
        private readonly AwsCredentials Options;

        private readonly AmazonS3Client s3Client = new AmazonS3Client(new AmazonS3Config()
        {
            RegionEndpoint = Amazon.RegionEndpoint.EUSouth1
        });


        public StorageService(IOptions<AwsCredentials> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public async Task<S3ResponseDto> UploadFileAsync(S3Models.S3Object s3object)
        {
            var credentials = new BasicAWSCredentials(Options.AwsKey, Options.AwsSecretKey);

            var response = new S3ResponseDto();

            try
            {
                var uploadRequest = new TransferUtilityUploadRequest()
                {
                    InputStream = s3object.InputStream,
                    Key = s3object.Name,
                    BucketName = s3object.BucketName,
                    CannedACL = S3CannedACL.NoACL
                };

                var transferUtility = new TransferUtility(s3Client);

                await transferUtility.UploadAsync(uploadRequest);

                response.StatusCode = 200;
                response.Message = $"{s3object.Name} has been uploaded successfully";
            }
            catch (AmazonS3Exception ex)
            {
                response.StatusCode = (int)ex.StatusCode;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
