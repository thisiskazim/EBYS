using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EBYS.WebAPI.Middlewares
{
    public class ValidationFilter : IAsyncActionFilter
    {

        private readonly IServiceProvider _serviceProvider;

        public ValidationFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 1. Controller'a gelen tüm parametreleri (DTO'ları) tek tek dönüyoruz
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument == null) continue;

                // 2. Bu DTO için yazılmış bir FluentValidation sınıfı (AbstractValidator) var mı diye sisteme soruyoruz
                var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
                var validator = _serviceProvider.GetService(validatorType) as IValidator;

                // 3. Eğer eşleşen bir validator sınıfı bulunduysa otomatik çalıştırıyoruz
                if (validator != null)
                {
                    var validationContext = new ValidationContext<object>(argument);
                    var validationResult = await validator.ValidateAsync(validationContext);

                    // 4. Hata varsa bizim GlobalExceptionHandler'ın yakalayacağı Exception'ı fırlatıp kodu kesiyoruz
                    if (!validationResult.IsValid)
                    {
                        throw new ValidationException(validationResult.Errors);
                    }
                }
            }

            // Hata yoksa controller metoduna güvenle devam et
            await next();
        }
    }
}
