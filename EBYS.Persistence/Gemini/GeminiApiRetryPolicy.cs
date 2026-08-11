using System.Net;
using System.Text.Json;

namespace EBYS.Persistence.Gemini
{
    internal static class GeminiApiRetryPolicy
    {
        private static readonly HashSet<HttpStatusCode> RetryableStatusCodes =
        [
            HttpStatusCode.TooManyRequests,      // 429
            HttpStatusCode.InternalServerError,  // 500
            HttpStatusCode.BadGateway,           // 502
            HttpStatusCode.ServiceUnavailable,   // 503
            HttpStatusCode.GatewayTimeout        // 504
        ];

        internal static bool IsRetryableStatusCode(HttpStatusCode statusCode) =>
            RetryableStatusCodes.Contains(statusCode);

        internal static bool IsNonRetryableStatusCode(HttpStatusCode statusCode) =>
            statusCode is HttpStatusCode.BadRequest
                or HttpStatusCode.Unauthorized
                or HttpStatusCode.Forbidden
                or HttpStatusCode.NotFound;

        internal static IEnumerable<string> GetModelCandidates(string primaryModel, IEnumerable<string>? fallbackModels)
        {
            var candidates = new List<string>();

            if (!string.IsNullOrWhiteSpace(primaryModel))
                candidates.Add(primaryModel.Trim());

            if (fallbackModels != null)
            {
                foreach (var model in fallbackModels)
                {
                    if (string.IsNullOrWhiteSpace(model))
                        continue;

                    var trimmed = model.Trim();
                    if (!candidates.Contains(trimmed, StringComparer.OrdinalIgnoreCase))
                        candidates.Add(trimmed);
                }
            }

            return candidates;
        }

        internal static string ExtractErrorMessage(string? errorBody)
        {
            if (string.IsNullOrWhiteSpace(errorBody))
                return "Bilinmeyen Gemini API hatası.";

            try
            {
                using var document = JsonDocument.Parse(errorBody);
                if (document.RootElement.TryGetProperty("error", out var errorElement) &&
                    errorElement.TryGetProperty("message", out var messageElement))
                {
                    return messageElement.GetString() ?? errorBody;
                }
            }
            catch (JsonException)
            {
                // Ham metni döndür
            }

            return errorBody.Length > 300 ? errorBody[..300] + "..." : errorBody;
        }
    }
}
