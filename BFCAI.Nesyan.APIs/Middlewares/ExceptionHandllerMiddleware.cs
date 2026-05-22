using BFCAI.Nesyan.Application.Common.Exceptions;
using BFCAI.Nesyan.Controllers.Errors;
using System.Net;

namespace BFCAI.Nesyan.APIs.Middlewares
{
    public class ExceptionHandllerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandllerMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionHandllerMiddleware(RequestDelegate next, ILogger<ExceptionHandllerMiddleware>  logger, IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _env = environment;
        }
        public async Task Invoke(HttpContext httpContent)
        {
            try
            {
                await _next(httpContent);
                if (httpContent.Response.StatusCode==(int)HttpStatusCode.NotFound)
                {
                    var response = new ApiResponse((int)HttpStatusCode.NotFound, $"The requested endpoint: {httpContent.Request.Path} is not found");

                    await httpContent.Response.WriteAsync(response.ToString());
                }
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    _logger.LogError(ex, ex.Message);
                else
                {

                }
                await HandleExceptionAsync(httpContent, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            ApiResponse response;
            switch (ex)
            {
                case NotFoundException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    httpContext.Response.ContentType = "application/json";
                    response = new ApiResponse(404, ex.Message);
                    await httpContext.Response.WriteAsync(response.ToString());
                    break;

                case ValidationException validationException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    httpContext.Response.ContentType = "application/json";
                    response = new ApiValidationErrorResponse(ex.Message) { Errors = (IEnumerable<ApiValidationErrorResponse.ValidationError>)validationException.Errors };
                    await httpContext.Response.WriteAsync(response.ToString());
                    break;

                case BadRequestException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    httpContext.Response.ContentType = "application/json";
                    response = new ApiResponse(404, ex.Message);
                    await httpContext.Response.WriteAsync(response.ToString());
                    break;

                case UnAuthourizedException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    httpContext.Response.ContentType = "application/json";
                    response = new ApiResponse(401, ex.Message);
                    await httpContext.Response.WriteAsync(response.ToString());
                    break;

                default:
                    response = _env.IsDevelopment() ?
                          response = new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.ToString()) :
                          response = new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsync(response.ToString());
                    break;
            }
        }
    }
}
