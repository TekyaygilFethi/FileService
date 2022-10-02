using Azure.Storage.Blobs;
using FileOperationsApplication.Data.FileServices;
using FileOperationsApplication.Data.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOperationsApplication.AzureProject
{
    public class AzureFileService : IFileService
    {
        string azureContainerName;
        string azureConnectionString;

        


        public AzureFileService(IConfiguration configuration)
        {
            azureContainerName = configuration.GetSection("AzureCredentials:ContainerName").Value;
            azureConnectionString = configuration.GetSection("AzureCredentials:ConnectionString").Value;
        }

        public async Task<bool> UploadAsync(UploadRequestModel request)
        {
            var container = new BlobContainerClient(azureConnectionString, azureContainerName);
            var blob = container.GetBlobClient(request.RemoteFilePath);
            using (var stream = request.File.OpenReadStream())
            {
                await blob.UploadAsync(stream);
            }

            return true;
        }

        public async Task<string> DownloadAsync(DownloadRequestModel request)
        {
            CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(azureConnectionString);
            CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference(azureContainerName);
            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(request.RemoteFilePath);
            return cloudBlockBlob.Uri.AbsoluteUri;
        
        }

        public Task DeleteAsync(DeleteRequestModel request)
        {
            throw new NotImplementedException();
        }
    }
}
