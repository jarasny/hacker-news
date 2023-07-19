namespace HackerNews.Models;

public class HackerNewsSettings
{
    public string? BaseAddress { get; set; }
    
    public int CacheExpirationInSeconds { get; set; }
}