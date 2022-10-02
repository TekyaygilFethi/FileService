using FileOperationsApplication.Data.FileServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOperationsApplication.Data.Shared
{
    public interface IFileService
    {
        Task<bool> UploadAsync(UploadRequestModel request);

        Task<string> DownloadAsync(DownloadRequestModel request);

        Task DeleteAsync(DeleteRequestModel request);
    }
}
