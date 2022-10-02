using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using System.Text;
using FileOperationsApplication.Data.Exceptions;
using FileOperationsApplication.Data.Shared;

namespace FileOperationsApplication.API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ControllerBase> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ControllerBase> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {

                var response = context.Response;
                response.ContentType = "application/json";

                var request = context.Request;
                HttpRequestRewindExtensions.EnableBuffering(request);
                var requestBody = string.Empty;


                using (StreamReader reader = new StreamReader(
            request.Body,
            Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            leaveOpen: true))
                {
                    // IMPORTANT: Reset the request body stream position so the next middleware can read it
                    request.Body.Position = 0;
                    requestBody = await reader.ReadToEndAsync();
                }


                _logger.LogError(error, $"Parametreler: Host: {context.Request.Host} - Controller: {context.Request.RouteValues["controller"]} - Action: {context.Request.RouteValues["action"]} - Path: {context.Request.Path} - Method: {context.Request.Method} - requestBody: {requestBody}");

                switch (error)
                {
                    case AppException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }


                var result = JsonSerializer.Serialize(new ResponseObject<object>
                {
                    Message = error.Message,
                    StatusCode = response.StatusCode,
                    IsSuccess = false
                });
                await response.WriteAsync(result);
            }
        }
    }
}
