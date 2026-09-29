namespace EBYS.WebAPI.Helpers;

public static class EnvLoader
{
    public static void Load(string contentRootPath)
    {
        var envPath = FindEnvFile(contentRootPath);
        if (envPath is null) return;

        foreach (var rawLine in File.ReadAllLines(envPath))
        {
            var line = rawLine.Trim();
            if (line.Length == 0 || line.StartsWith('#')) continue;

            var separator = line.IndexOf('=');
            if (separator <= 0) continue;

            var key = line[..separator].Trim();
            var value = line[(separator + 1)..].Trim().Trim('"');
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(key)))
                Environment.SetEnvironmentVariable(key, value);
        }

        MapToAspNetConfig();
    }

    private static string? FindEnvFile(string startPath)
    {
        var dir = new DirectoryInfo(startPath);
        while (dir is not null)
        {
            var candidate = Path.Combine(dir.FullName, ".env");
            if (File.Exists(candidate)) return candidate;
            dir = dir.Parent;
        }
        return null;
    }

    private static void MapToAspNetConfig()
    {
        SetIfEmpty("JwtSettings__Secret", Environment.GetEnvironmentVariable("JWT_SECRET"));
        SetIfEmpty("JwtSettings__Issuer", Environment.GetEnvironmentVariable("JWT_ISSUER"));
        SetIfEmpty("JwtSettings__Audience", Environment.GetEnvironmentVariable("JWT_AUDIENCE"));
        SetIfEmpty("GeminiSettings__ApiKey", Environment.GetEnvironmentVariable("GEMINI_API_KEY"));
    }

    private static void SetIfEmpty(string aspNetKey, string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return;
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(aspNetKey)))
            Environment.SetEnvironmentVariable(aspNetKey, value);
    }
}
