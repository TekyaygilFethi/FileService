using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOperationsApplication.Data.FileServices
{
    public class UploadRequestModel
    {
        public IFormFile File { get; set; }
        public string RemoteFilePath { get; set; }
    }
}
