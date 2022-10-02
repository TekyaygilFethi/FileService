using FileOperationsApplication.Data.FileServices;
using FileOperationsApplication.Data.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOperationsApplication.Data.FileControllers
{
    public class DownloadControllerRequestModel: DownloadRequestModel
    {
        public FileDestinationServiceEnum FileDestination { get; set; }
    }
}
