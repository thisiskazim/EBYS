
using EBYS.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EBYS.WebAPI.Handler
{
    public class GlobalExceptionHandler: IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
        {
          
            _logger.LogError(exception, "Uygulamada bir hata fırladı: {Message}", exception.Message);

            int statusCode = StatusCodes.Status500InternalServerError;
            string title = "Sunucu Hatası";
            string detail = "Beklenmedik sistemsel bir hata oluştu.";

            if (exception is BaseException baseException)
            {
                statusCode = baseException.StatusCode;
                title = exception.GetType().Name; 
                detail = exception.Message;       
            }

  
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path
            };

           
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
        }
}
