using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOperationsApplication.Data.FileServices
{
    public class DeleteRequestModel
    {
        public string FilePath { get; set; }
        public string? VersionId { get; set; } = null;
    }
}
