namespace EBYS.Persistence.Gemini.Options
{
    public class GeminiSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string Model { get; set; } = "gemini-2.0-flash";
        public List<string> FallbackModels { get; set; } = [];
        public int MaxOutputTokens { get; set; } = 4096;
        public string BaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1beta";
        public int MaxRetryAttempts { get; set; } = 2;
        public int RetryDelayMilliseconds { get; set; } = 1500;
    }
}
