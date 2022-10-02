using FileOperationsApplication.Business.Shared;
using FileOperationsApplication.Data.FileControllers;
using FileOperationsApplication.Data.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileOperationsApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        IConfiguration _configuration;
        public FileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost, Route("Download")]
        public async Task<ResponseObject<string>> Download([FromBody]DownloadControllerRequestModel request)
        {
            var fileService = FileServiceCreator.GetFileService(request.FileDestination, _configuration);

            var data = await fileService.DownloadAsync(request);

            return new ResponseObject<string>()
            {
                Data = data
            };
        }

        [HttpPost, Route("Upload")]
        public async Task<ResponseObject<bool>> Upload([FromForm]UploadControllerRequestModel request)
        {
            var fileService = FileServiceCreator.GetFileService(request.FileDestination, _configuration);

            var data = await fileService.UploadAsync(request);

            return new ResponseObject<bool>()
            {
                Data = data
            };
        }
    }
}
