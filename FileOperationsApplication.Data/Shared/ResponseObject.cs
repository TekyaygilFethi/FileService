using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOperationsApplication.Data.Shared
{
    public class ResponseObject<T>
    {
        public bool IsSuccess { get; set; } = true;

        public int StatusCode { get; set; } = 200;

        public string Message { get; set; } = "Success";

        public T Data { get; set; }
    }
}
