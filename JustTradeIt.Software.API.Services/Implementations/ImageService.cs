using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using JustTradeIt.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace JustTradeIt.Software.API.Services.Implementations
{
    public class ImageService : IImageService
    {
        private readonly IConfiguration _configuration;

        public ImageService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public async Task<string> UploadImageToBucket(string email, IFormFile image)
        {

            try
            {
            var bucketName = _configuration["AwsSettings:BucketName"];

            var credentials = new BasicAWSCredentials(_configuration["AwsSettings:AccessKey"], _configuration["AwsSettings:SecreKey"]);
                var config = new AmazonS3Config
                {
                    RegionEndpoint = Amazon.RegionEndpoint.EUWest1
                };
                using var client = new AmazonS3Client(credentials, config);
                await using var newMemoryStream = new MemoryStream();
                await image.CopyToAsync(newMemoryStream);
                var fileExtension = Path.GetExtension(image.FileName);
                var documentName = $"{RandomString(email)}{fileExtension}";
                var result = $"https://{bucketName}.s3.amazonaws.com/{documentName}";
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = newMemoryStream,
                    Key = documentName,
                    BucketName = bucketName,
                    CannedACL = S3CannedACL.PublicRead
                };
                var fileTransferUtility = new TransferUtility(client);
                await fileTransferUtility.UploadAsync(uploadRequest);
                return result;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                System.Diagnostics.Debug.WriteLine(amazonS3Exception.Message);
                return "error";
            }

        }
        public string RandomString(string email)
        {
            Random random = new Random();
             string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"+email.Substring(0,10);
            return new string(System.Linq.Enumerable.Repeat(chars, 10)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}