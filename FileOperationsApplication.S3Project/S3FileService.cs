using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using FileOperationsApplication.Data.FileServices;
using FileOperationsApplication.Data.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FileOperationsApplication.S3Project
{
    public class S3FileService : IFileService
    {
        //private IConfiguration _configuration;
        private readonly string _bucketName;
        private readonly IAmazonS3 _awsS3Client;

        private readonly string _awsAccessKey;
        private readonly string _awsSecretAccessKey;

        private readonly string REGION = "us-east-1";

        public S3FileService(IConfiguration configuration)
        {
            _awsAccessKey = configuration.GetSection("AmazonS3Credentials:AwsAccessKey").Value;
            _awsSecretAccessKey = configuration.GetSection("AmazonS3Credentials:AwsSecretAccessKey").Value;

            _bucketName = configuration.GetSection("AmazonS3Credentials:BucketName").Value;
            _awsS3Client = new AmazonS3Client(_awsAccessKey, _awsSecretAccessKey, RegionEndpoint.GetBySystemName(REGION));
        }

        public async Task DeleteAsync(DeleteRequestModel request)
        {
            DeleteObjectRequest deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = request.FilePath
            };

            if (!string.IsNullOrEmpty(request.VersionId))
                request.VersionId = request.VersionId;

            await _awsS3Client.DeleteObjectAsync(deleteObjectRequest);
        }

        private async Task<bool> IsFileExists(string fileName, string versionId = null)
        {
            try
            {
                GetObjectMetadataRequest request = new GetObjectMetadataRequest()
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    VersionId = !string.IsNullOrEmpty(versionId) ? versionId : null
                };

                var response = await _awsS3Client.GetObjectMetadataAsync(request);

                return true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is AmazonS3Exception awsEx)
                {
                    if (string.Equals(awsEx.ErrorCode, "NoSuchBucket"))
                        return false;

                    else if (string.Equals(awsEx.ErrorCode, "NotFound"))
                        return false;
                }

                throw;
            }
        }

        public async Task<string> DownloadAsync(DownloadRequestModel request)
        {
            MemoryStream ms = null;


            //GetObjectRequest getObjectRequest = new GetObjectRequest
            //{
            //    BucketName = _bucketName,
            //    Key = request.RemoteFilePath
            //};

            if (await IsFileExists(request.RemoteFilePath))
                return "https://" + _bucketName + ".s3.amazonaws.com/" + request.RemoteFilePath;
            else
                throw new FileNotFoundException(string.Format("The document '{0}' is not found", request.RemoteFilePath));

            //using (var response = await _awsS3Client.Url(getObjectRequest))
            //{
            //    if (response.HttpStatusCode == HttpStatusCode.OK)
            //    {
            //        return response.ResponseStream;
            //        //Stream file = System.IO.File.OpenWrite(request.RemoteFilePath);
            //        //await response.ResponseStream.CopyToAsync(file);
            //        //return file;
            //    }
            //    else
            //        throw new Exception("Error while downloading from Amazon Server! Http Code:" + response.HttpStatusCode);
            //}

            //if (ms is null || ms.ToArray().Length < 1)
            //    throw new FileNotFoundException(string.Format("The document '{0}' is not found", request.RemoteFilePath));
        }

        public async Task<bool> UploadAsync(UploadRequestModel request)
        {
            var fileTransferUtility = new TransferUtility(_awsS3Client);

            using (var newMemoryStream = new MemoryStream())
            {
                request.File.CopyTo(newMemoryStream);

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = newMemoryStream,
                    Key = request.RemoteFilePath,
                    BucketName = _bucketName,
                    ContentType = request.File.ContentType
                };

                await fileTransferUtility.UploadAsync(uploadRequest);
            }

            return true;

        }
    }
}
