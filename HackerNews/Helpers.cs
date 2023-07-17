using System.Text.Json;

namespace HackerNews;

public static class Helpers
{
    public static JsonSerializerOptions DefaultJsonSerializerOptions => new ()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}