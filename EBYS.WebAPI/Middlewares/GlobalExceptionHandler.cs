using EBYS.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace EBYS.WebAPI.Middlewares
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
            _logger.LogError(exception, "Hata Yakalandı: {Message}", exception.Message);

            int statusCode = 500;
            string title = "Sistem Hatası";
            object detail = "Beklenmedik bir hata oluştu.";

            switch (exception)
            {
                case FluentValidation.ValidationException valEx:
                    statusCode = 400;
                    title = "Validasyon Hatası";
                    detail = valEx.Errors.Select(e => new {
                        field = e.PropertyName,
                        message = e.ErrorMessage
                    }).ToList();
                    break;

                case NotFoundException nfEx:
                    statusCode = nfEx.StatusCode;
                    title = "Kaynak Bulunamadı";
                    detail = nfEx.Message;
                    break;

                case BusinessException busEx:
                    statusCode = busEx.StatusCode;
                    title = "İş Kuralı İhlali";
                    detail = busEx.Message;
                    break;
            }

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            var response = new { Title = title, Status = statusCode, Detail = detail };
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return true;
        }
    }
}
