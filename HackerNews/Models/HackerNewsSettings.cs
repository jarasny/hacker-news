namespace HackerNews.Models;

public class HackerNewsSettings
{
    public string? BaseAddress { get; set; }
    
    public int StoriesListCacheExpirationInSeconds { get; set; }
    
    public int StoryCacheExpirationInSeconds { get; set; }
}