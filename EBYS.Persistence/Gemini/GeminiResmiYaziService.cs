using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using EBYS.Application.DTOs.ResmiYaziDTO;
using EBYS.Application.Interfaces.IService.IResmiYaziService;
using EBYS.Domain.Exceptions;
using EBYS.Persistence.Gemini.Instructions;
using EBYS.Persistence.Gemini.Models;
using EBYS.Persistence.Gemini.Options;
using Microsoft.Extensions.Options;

namespace EBYS.Persistence.Gemini
{
    public class GeminiResmiYaziService(
        HttpClient httpClient,
        IOptions<GeminiSettings> options,
        ResmiYaziSystemInstructionFactory instructionFactory) : IResmiYaziGeneratorService
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        public async Task<ResmiYaziGenerateResponse> ResmiMetinOlusturAsync(ResmiYaziGenerateRequest request)
        {
            var settings = options.Value;

            if (string.IsNullOrWhiteSpace(settings.ApiKey))
                throw new GeminiServisHatasi("Gemini API anahtarı yapılandırılmamış.");

            var systemInstruction = instructionFactory.GetSystemInstruction(
                request.YaziTuru,
                request.YaziUzunlugu);

            var apiRequest = BuildApiRequest(systemInstruction, request.TaslakMetin, settings.MaxOutputTokens);
            var modelCandidates = GeminiApiRetryPolicy
                .GetModelCandidates(settings.Model, settings.FallbackModels)
                .ToList();

            if (modelCandidates.Count == 0)
                throw new GeminiServisHatasi("Yapılandırılmış Gemini modeli bulunamadı.");

            var errors = new List<string>();

            try
            {
                foreach (var model in modelCandidates)
                {
                    for (var attempt = 1; attempt <= Math.Max(1, settings.MaxRetryAttempts); attempt++)
                    {
                        var result = await TryGenerateAsync(settings, model, apiRequest);

                        if (result.Success)
                        {
                            var parsed = ParseAiOutput(result.GeneratedText!);

                            if (string.IsNullOrWhiteSpace(parsed.Konu) && string.IsNullOrWhiteSpace(parsed.Icerik))
                                throw new GeminiServisHatasi("Gemini API geçerli bir konu veya içerik üretemedi.");

                            return new ResmiYaziGenerateResponse
                            {
                                YaziTuru = request.YaziTuru,
                                Konu = parsed.Konu.Trim(),
                                ResmiMetin = parsed.Icerik.Trim()
                            };
                        }

                        errors.Add($"[{model} - deneme {attempt}] {result.ErrorMessage}");

                        if (result.ShouldStopAll)
                            throw new GeminiServisHatasi(result.ErrorMessage);

                        if (result.ShouldRetry && attempt < settings.MaxRetryAttempts)
                        {
                            await Task.Delay(settings.RetryDelayMilliseconds * attempt);
                            continue;
                        }

                        break;
                    }
                }

                throw new GeminiServisHatasi(
                    "Gemini servisi şu anda yoğun. Birkaç dakika sonra tekrar deneyiniz. " +
                    $"Denenen modeller: {string.Join(", ", modelCandidates)}. " +
                    $"Son hata: {errors.LastOrDefault() ?? "Bilinmeyen hata"}");
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex) when (ex is not GeminiServisHatasi)
            {
                throw new GeminiServisHatasi($"Gemini API hatası: {ex.Message}");
            }
        }

        private static GeminiGenerateContentRequest BuildApiRequest(
            string systemInstruction,
            string taslakMetin,
            int maxOutputTokens) =>
            new()
            {
                SystemInstruction = new GeminiContent
                {
                    Parts = [new GeminiPart { Text = systemInstruction }]
                },
                Contents =
                [
                    new GeminiContent
                    {
                        Parts = [new GeminiPart { Text = taslakMetin }]
                    }
                ],
                GenerationConfig = new GeminiGenerationConfig
                {
                    MaxOutputTokens = maxOutputTokens,
                    Temperature = 0.3f,
                    ResponseMimeType = "application/json"
                }
            };

        private async Task<GeminiApiAttemptResult> TryGenerateAsync(
            GeminiSettings settings,
            string model,
            GeminiGenerateContentRequest apiRequest)
        {
            var requestUri =
                $"{settings.BaseUrl.TrimEnd('/')}/models/{model}:generateContent?key={settings.ApiKey}";

            try
            {
                using var response = await httpClient.PostAsJsonAsync(requestUri, apiRequest, JsonOptions);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    var errorMessage = GeminiApiRetryPolicy.ExtractErrorMessage(errorBody);
                    var statusCode = response.StatusCode;

                    if (GeminiApiRetryPolicy.IsNonRetryableStatusCode(statusCode))
                    {
                        return GeminiApiAttemptResult.StopAll(
                            $"Gemini API hatası ({(int)statusCode}): {errorMessage}");
                    }

                    if (GeminiApiRetryPolicy.IsRetryableStatusCode(statusCode))
                    {
                        return GeminiApiAttemptResult.Retry(
                            $"Gemini API geçici hata ({(int)statusCode}): {errorMessage}");
                    }

                    return GeminiApiAttemptResult.Fail(
                        $"Gemini API hatası ({(int)statusCode}): {errorMessage}");
                }

                var apiResponse = await response.Content.ReadFromJsonAsync<GeminiGenerateContentResponse>(JsonOptions);
                var generatedText = apiResponse?.Candidates?
                    .FirstOrDefault()?.Content?.Parts?
                    .FirstOrDefault()?.Text;

                if (string.IsNullOrWhiteSpace(generatedText))
                    return GeminiApiAttemptResult.Fail("Gemini API geçerli bir metin üretemedi.");

                return GeminiApiAttemptResult.Ok(generatedText);
            }
            catch (TaskCanceledException)
            {
                return GeminiApiAttemptResult.Retry("Gemini API isteği zaman aşımına uğradı.");
            }
            catch (HttpRequestException ex)
            {
                return GeminiApiAttemptResult.Retry($"Bağlantı hatası: {ex.Message}");
            }
        }

        private static ResmiYaziAiOutput ParseAiOutput(string generatedText)
        {
            var jsonText = ExtractJsonObject(generatedText);

            try
            {
                var parsed = JsonSerializer.Deserialize<ResmiYaziAiOutput>(jsonText, JsonOptions);
                if (parsed != null)
                    return parsed;
            }
            catch (JsonException)
            {
                // Fallback below
            }

            return new ResmiYaziAiOutput
            {
                Konu = string.Empty,
                Icerik = generatedText.Trim()
            };
        }

        private static string ExtractJsonObject(string text)
        {
            var trimmed = text.Trim();

            if (trimmed.StartsWith("```", StringComparison.Ordinal))
            {
                trimmed = Regex.Replace(trimmed, "^```(?:json)?\\s*", "", RegexOptions.IgnoreCase);
                trimmed = Regex.Replace(trimmed, "\\s*```$", "");
            }

            var start = trimmed.IndexOf('{');
            var end = trimmed.LastIndexOf('}');

            if (start >= 0 && end > start)
                return trimmed[start..(end + 1)];

            return trimmed;
        }

        private sealed class GeminiApiAttemptResult
        {
            public bool Success { get; init; }
            public string? GeneratedText { get; init; }
            public string ErrorMessage { get; init; } = string.Empty;
            public bool ShouldRetry { get; init; }
            public bool ShouldStopAll { get; init; }

            public static GeminiApiAttemptResult Ok(string text) =>
                new() { Success = true, GeneratedText = text };

            public static GeminiApiAttemptResult Retry(string message) =>
                new() { ErrorMessage = message, ShouldRetry = true };

            public static GeminiApiAttemptResult Fail(string message) =>
                new() { ErrorMessage = message };

            public static GeminiApiAttemptResult StopAll(string message) =>
                new() { ErrorMessage = message, ShouldStopAll = true };
        }
    }
}
