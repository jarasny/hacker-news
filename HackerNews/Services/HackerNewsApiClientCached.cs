using HackerNews.Models;
using LazyCache;
using Microsoft.Extensions.Options;

namespace HackerNews.Services;

public class HackerNewsApiClientCached : IHackerNewsApiClient
{
    private readonly IHackerNewsApiClient _hackerNewsApiClient;
    private readonly IAppCache _cache;
    private readonly HackerNewsSettings _settings;
    private const string StoriesIdsCacheKey = "stories-ids";

    public HackerNewsApiClientCached(
        IHackerNewsApiClient hackerNewsApiClient, 
        IAppCache cache, 
        IOptions<HackerNewsSettings> settings)
    {
        _hackerNewsApiClient = hackerNewsApiClient;
        _cache = cache;
        _settings = settings.Value;
    }
    
    public async Task<HackerNewsStoryDto?> GetStory(int id)
    {
        return await _cache.GetOrAddAsync(
            id.ToString(), 
            async () => await _hackerNewsApiClient.GetStory(id),
            TimeSpan.FromSeconds(_settings.StoryCacheExpirationInSeconds));
    }

    public async Task<IEnumerable<int>?> GetBestStoriesIds()
    {
        return await _cache.GetOrAddAsync(StoriesIdsCacheKey, 
            async () => await _hackerNewsApiClient.GetBestStoriesIds(),
            DateTimeOffset.Now.AddSeconds(_settings.StoriesListCacheExpirationInSeconds));
    }
}