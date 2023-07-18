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
    
    public async Task<HackerNewsStoryDto?> GetStoryAsync(int id)
    {
        return await _cache.GetOrAddAsync(
            id.ToString(), 
            async () => await _hackerNewsApiClient.GetStoryAsync(id),
            TimeSpan.FromSeconds(_settings.StoryCacheExpirationInSeconds));
    }

    public async Task<IEnumerable<int>?> GetBestStoriesIdsAsync()
    {
        return await _cache.GetOrAddAsync(StoriesIdsCacheKey, 
            async () => await _hackerNewsApiClient.GetBestStoriesIdsAsync(),
            DateTimeOffset.Now.AddSeconds(_settings.StoriesListCacheExpirationInSeconds));
    }
}