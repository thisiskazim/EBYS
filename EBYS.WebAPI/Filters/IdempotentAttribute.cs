using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StackExchange.Redis;


namespace EBYS.WebAPI.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class IdempotentAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _lockExpirySeconds;

        public IdempotentAttribute(int lockExpirySeconds = 30)
        {
            _lockExpirySeconds = lockExpirySeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 1. İstemciden gelen anahtarı kontrol et
            if (!context.HttpContext.Request.Headers.TryGetValue("Idempotency-Key", out var idempotencyKey))
            {
                context.Result = new BadRequestObjectResult("Idempotency-Key header'ı zorunludur!");
                return;
            }

            // Dependency Injection üzerinden Redis servisini al
            var redis = context.HttpContext.RequestServices.GetRequiredService<IConnectionMultiplexer>();
            var db = redis.GetDatabase();
            string redisKey = $"idempotency_lock:{idempotencyKey}";

            // 2. Redis'e Kilidi At (Atomic İşlem)
            // When.NotExists (NX): Bu anahtar Redis'te YOKSA kaydet ve TRUE dön. VARSA FALSE dön.
            // Bu işlem Race Condition (Yarış Durumu) ihtimalini sıfıra indirir.
            bool isLockAcquired = await db.StringSetAsync(
                redisKey,
                "processing",
                TimeSpan.FromSeconds(_lockExpirySeconds),
                When.NotExists
            );

            if (!isLockAcquired)
            {
                // 3. İstek zaten içeride işleniyor! Mükerrer isteği reddet.
                context.Result = new ConflictObjectResult("Bu işlem şu anda gerçekleştiriliyor. Lütfen bekleyin.");
                return;
            }

            // 4. Kilit başarıyla alındıysa, asıl işlemi (Controller'ı) çalıştır
            await next();

            // Not: Gerçek dünyada işlem bitince Redis'teki değer "completed" olarak güncellenip 
            // sonuç cache'lenebilir. Ancak kilit mantığı mükerrerliği kesmek için yeterlidir.
        }
    }
}
